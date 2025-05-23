using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; 

    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("hhhh");
        if (other.CompareTag("Player"))
        {
            Debug.Log("colliddss");
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            if (inventory != null && inventory.AddItem(itemData))
            {
                Destroy(gameObject); // Remove the item from the world
            }
        }
    }
}