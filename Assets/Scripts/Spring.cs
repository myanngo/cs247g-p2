using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Item Configuration")]
    public Item filledBottleItem;
    
    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip fillSound;
    
    [Header("Visual Effects (Optional)")]
    public GameObject springAnimator;

    [Header("Inventory Reward")]
    public Item bottleItem; 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Globals.StoryStage == 2)
        {
            FillBottle();
        }
    }
    
    private void FillBottle()
    {
        // Check if player has any empty bottles
        if (HasEmptyBottle())
        {
            InventoryData.Instance.inventory.RemoveAll(entry => entry.item.type == ItemType.Bottle);
                
            // Add one bottle
            InventoryData.Instance.AddItem(bottleItem);
                
            // Update UI if InventoryManager exists
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RefreshInventoryUI();
            }

            // Play effects
            PlayEffects();
            
            Debug.Log("Bottle filled with spring water!");
        }
        else
        {
            Debug.Log("You need an empty bottle to fill!");
        }
    }
    
    private bool HasEmptyBottle()
    {
        if (InventoryData.Instance != null)
        {
            return InventoryData.Instance.CountItem(ItemType.Bottle) > 0;
        }
        return false;
    }
    
    private void RemoveEmptyBottle()
    {
        if (InventoryData.Instance != null)
        {
            // Find and remove one bottle from inventory data
            var bottleEntry = InventoryData.Instance.inventory.Find(entry => entry.item.type == ItemType.Bottle);
            if (bottleEntry != null)
            {
                bottleEntry.count--;
                if (bottleEntry.count <= 0)
                {
                    InventoryData.Instance.inventory.Remove(bottleEntry);
                }
                
                // Update UI if InventoryManager exists
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.RefreshInventoryUI();
                }
            }
        }
    }
    
    private void AddFilledBottle()
    {
        if (filledBottleItem != null)
        {
            // Add to inventory data
            if (InventoryData.Instance != null)
            {
                InventoryData.Instance.AddItem(filledBottleItem);
            }
            
            // Update UI if InventoryManager exists
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RefreshInventoryUI();
            }
        }
        else
        {
            Debug.LogError("Spring: FilledBottle item is not assigned!");
        }
    }
    
    private void PlayEffects()
    {
        // Play sound effect
        if (audioSource != null && fillSound != null)
        {
            audioSource.PlayOneShot(fillSound);
        }
        
        // Play animation
        if (springAnimator != null)
        {
            springAnimator.SetActive(true);
        }
    }
}