using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;
    public string itemID; // unique identifier

    void Start()
    {
        // If item was already picked up in a previous session, destroy it
        if (Globals.collectedItemIDs.Contains(itemID))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to use InventoryManager first (for UI updates)
            if (InventoryManager.Instance != null)
            {
                if (InventoryManager.Instance.AddItem(itemData))
                {
                    Globals.collectedItemIDs.Add(itemID); // Mark as collected
                    Destroy(gameObject);
                    InventoryManager.Instance.TriggerPickupAnimation();
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
            // Fallback to InventoryData if no InventoryManager (in minigame scenes)
            else if (InventoryData.Instance != null)
            {
                if (InventoryData.Instance.AddItem(itemData))
                {
                    Globals.collectedItemIDs.Add(itemID); // Mark as collected
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full!");
                }
            }
        }
    }
}