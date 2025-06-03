using UnityEngine;
using System.Collections.Generic;

public class InventoryData : MonoBehaviour
{
    public static InventoryData Instance;

    [System.Serializable]
    public class InventoryEntry
    {
        public Item item;
        public int count;
    }

    public List<InventoryEntry> inventory = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(Item item)
    {
        var existing = inventory.Find(entry => entry.item == item);
        if (existing != null)
        {
            existing.count++;
        }
        else
        {
            inventory.Add(new InventoryEntry { item = item, count = 1 });
        }
        return true;
    }

    public int CountItem(ItemType type)
    {
        int total = 0;
        foreach (var entry in inventory)
        {
            if (entry.item.type == type)
            {
                total += entry.count;
            }
        }
        return total;
    }
}
