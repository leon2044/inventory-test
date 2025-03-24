using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

    public List<GameObject> itemSlots = new List<GameObject>(); // ذخیره اسلات‌های UI

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 🚀 تعداد اسلات‌های UI را با تعداد آیتم‌های اینونتوری هماهنگ کنیم
        while (itemSlots.Count < inventory.items.Count)
        {
            CreateItemSlot(inventory.items[itemSlots.Count]); // فقط آیتم‌های جدید را اضافه کن
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            itemSlots[i].transform.Find("ItemIcon").GetComponent<Image>().sprite = inventory.items[i].icon;
        }
    }

    private void CreateItemSlot(DragObject item)
    {
        GameObject slot = Instantiate(itemSlotPrefab, inventoryPanel);
        slot.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;

        ItemSlotHandler handler = slot.AddComponent<ItemSlotHandler>();
        handler.inventoryUI = this;
        handler.item = item;
        handler.slotIndex = itemSlots.Count;

        itemSlots.Add(slot);
    }

    public void AddItemToUI(DragObject newItem)
    {
        inventory.items.Add(newItem);
        CreateItemSlot(newItem);
    }

    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index2 < 0 || index1 >= inventory.items.Count || index2 >= inventory.items.Count)
            return;

        // 🚀 فقط جای آیتم‌ها در لیست جابه‌جا کن
        DragObject temp = inventory.items[index1];
        inventory.items[index1] = inventory.items[index2];
        inventory.items[index2] = temp;

        // 🚀 آیکون‌های UI را جابه‌جا کن بدون ساخت آیتم جدید
        Transform slot1 = itemSlots[index1].transform;
        Transform slot2 = itemSlots[index2].transform;

        Sprite tempSprite = slot1.Find("ItemIcon").GetComponent<Image>().sprite;
        slot1.Find("ItemIcon").GetComponent<Image>().sprite = slot2.Find("ItemIcon").GetComponent<Image>().sprite;
        slot2.Find("ItemIcon").GetComponent<Image>().sprite = tempSprite;
    }


    public void RemoveItem(int index)
    {
        if (index < 0 || index >= inventory.items.Count)
            return;

        Debug.Log($"🗑️ در حال حذف آیتم: {inventory.items[index]._Name}");

        // بازگرداندن آیتم به محیط بازی
        inventory.items[index].gameObject.SetActive(true);

        // حذف از لیست اینونتوری
        inventory.items.RemoveAt(index);

        // حذف از UI
        Destroy(itemSlots[index]);
        itemSlots.RemoveAt(index);

        // 🚀 اصلاح ایندکس‌های باقیمانده برای جلوگیری از بهم ریختگی
        for (int i = index; i < itemSlots.Count; i++)
        {
            itemSlots[i].GetComponent<ItemSlotHandler>().slotIndex = i;
        }

        UpdateUI();
    }


}
