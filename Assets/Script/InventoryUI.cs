using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public GameObject itemSlotPrefab;
    public Transform inventoryPanel;

    public List<GameObject> itemSlots = new List<GameObject>(); // Save UI slots

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Match the number of UI slots to the number of inventory items
        while (itemSlots.Count < inventory.items.Count)
        {
            CreateItemSlot(inventory.items[itemSlots.Count]); // Only add new items
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



        DragObject temp = inventory.items[index1];
        inventory.items[index1] = inventory.items[index2];
        inventory.items[index2] = temp;

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

        inventory.items[index].gameObject.SetActive(true);

        inventory.items.RemoveAt(index);

        Destroy(itemSlots[index]);
        itemSlots.RemoveAt(index);


        for (int i = index; i < itemSlots.Count; i++)
        {
            itemSlots[i].GetComponent<ItemSlotHandler>().slotIndex = i;
        }

        UpdateUI();
    }


}
