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
        // advance stage when get lily of the valley
        if (item.type == ItemType.LilyofValley)
        {
            Globals.StoryStage = 4;
        }
        // advance stage when get filled bottle from frog
        if (item.type == ItemType.FilledBottle)
        {
            Globals.StoryStage = 3;
        }
        
        // advance stage when get flower from bear
        if (item.type == ItemType.Flower)
        {
            Globals.StoryStage = 5;
        }

        // replace flower with honey
        if (item.type == ItemType.Honey)
        {
            var flowerToReplace = inventory.Find(entry => entry.item.type == ItemType.Flower && entry.item.actionType == ActionType.Exchange);
            if (flowerToReplace != null)
            {
                inventory.Remove(flowerToReplace); // remove flower
                Debug.Log("Replaced Flower (Exchange) with Honey");
            }
        }

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
