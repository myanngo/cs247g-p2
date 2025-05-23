using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; 

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = other.GetComponent<InventoryManager>();
            if (inventory != null && inventory.AddItem(itemData))
            {
                Destroy(gameObject); // Remove the item from the world
            }
        }
    }
}