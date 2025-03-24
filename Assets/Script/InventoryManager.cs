using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    public void Start()
    {
        LoadInventory();
    }

    public void SaveInventory()
    {
        List<string> itemNames = inventory.items.Select(item => item._Name).ToList();
        string json = JsonUtility.ToJson(new SerializableList<string>(itemNames));
        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            List<string> itemNames = JsonUtility.FromJson<SerializableList<string>>(json).list;

            foreach (string itemName in itemNames)
            {
                DragObject item = FindItemByName(itemName);
                if (item != null)
                {
                    inventory.AddItem(item);
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    private DragObject FindItemByName(string name)
    {
        return FindObjectsOfType<DragObject>().FirstOrDefault(item => item._Name == name);
    }

    [System.Serializable]
    private class SerializableList<T>
    {
        public List<T> list;
        public SerializableList(List<T> list) { this.list = list; }
    }
}
