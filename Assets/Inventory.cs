using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

public class Inventory : MonoBehaviour
{
    public List<DragObject> items = new List<DragObject>();
    public InventoryManager _inventoryManager;

    private static readonly HttpClient client = new HttpClient(); // برای ارسال درخواست HTTP
    private string serverUrl = "https://wadahub.manerai.com/api/inventory/status";
    private string authToken = "kPERnYcWAY46xaSy8CEzanosAgsWMB4Nx7SKM4QBSqPq6c7StWfGxzhxPfb8MaP"; // توکن احراز هویت

    public GameObject _canvasobject;

    public async void AddItem(DragObject newItem)
    {
        if (!items.Contains(newItem))
        {
            items.Add(newItem);
            FindObjectOfType<InventoryUI>().UpdateUI();
            Debug.Log($"Added {newItem._Name} to inventory.");

            _inventoryManager.SaveInventory();



            await SendItemToServer(newItem, "added");
        }
    }

    public async void RemoveItem(DragObject itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            Debug.Log($"Trying to remove: {itemToRemove._Name}");

            items.Remove(itemToRemove); // حذف آیتم به‌طور دقیق

            // پاک کردن از UI
            FindObjectOfType<InventoryUI>().UpdateUI();

            Debug.Log($"Removed {itemToRemove._Name} from inventory. Remaining items: {items.Count}");

            await SendItemToServer(itemToRemove, "removed"); // ارسال اطلاعات حذف به سرور
        }
        else
        {
            Debug.LogError("⚠️ آیتم موردنظر در لیست پیدا نشد!");
        }
    }

    private async Task SendItemToServer(DragObject item, string status)
    {
        string json = $"{{\"id\": {item.Id}, \"name\": \"{item._Name}\", \"status\": \"{status}\"}}";
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        // اضافه کردن توکن احراز هویت
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

        try
        {
            HttpResponseMessage response = await client.PostAsync(serverUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();
            Debug.Log($"📡 Server Response: {responseString}");
        }
        catch (HttpRequestException e)
        {
            Debug.LogError($"❌ Error sending data to server: {e.Message}");
        }
    }


    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // وقتی کلیک چپ زده شد
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform) // اگر روی همین آبجکت کلیک شده بود
                {
                    _canvasobject.SetActive(!_canvasobject.activeSelf); // آبجکت را روشن کن
                    Debug.Log("آبجکت روشن شد!");
                }
            }
        }
    }

}
