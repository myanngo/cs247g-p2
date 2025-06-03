using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Item Configuration")]
    public Item filledBottleItem; // Drag your FilledBottle ScriptableObject here
    
    [Header("Audio (Optional)")]
    public AudioSource audioSource;
    public AudioClip fillSound;
    
    [Header("Visual Effects (Optional)")]
    public ParticleSystem fillEffect;
    public Animator springAnimator;
    
    private void OnMouseDown()
    {
        FillBottle();
    }
    
    // Alternative method if you're using a different input system
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            FillBottle();
        }
    }
    
    private void FillBottle()
    {
        // Check if player has any empty bottles
        if (HasEmptyBottle())
        {
            // Remove one empty bottle
            RemoveEmptyBottle();
            
            // Add filled bottle
            AddFilledBottle();
            
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
        
        // Play particle effect
        if (fillEffect != null)
        {
            fillEffect.Play();
        }
        
        // Play animation
        if (springAnimator != null)
        {
            springAnimator.SetTrigger("Fill");
        }
    }
}