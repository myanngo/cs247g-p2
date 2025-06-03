using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance; // Singleton instance

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    [Header("Item Detail UI")]
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public Image itemDetailImage;
    public GameObject itemImageContainer;
    [Header("Pickup Animation")]
    public Animator inventoryCanvasAnimator; // Animator for the inventory canvas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Hide item details at start
        ClearItemDetails();
        
        // Load existing inventory data when the scene starts
        LoadInventoryFromData();
    }

    // Load inventory from persistent data
    private void LoadInventoryFromData()
    {
        if (InventoryData.Instance == null) return;

        // Clear existing UI items
        ClearInventoryUI();

        // Populate UI with data from InventoryData
        foreach (var entry in InventoryData.Instance.inventory)
        {
            AddItemToUI(entry.item, entry.count);
        }
    }

    // Clear all items from UI
    private void ClearInventoryUI()
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                DestroyImmediate(itemInSlot.gameObject);
            }
        }
    }

    // This method now updates the UI to match InventoryData
    public bool AddItem(Item item)
    {
        // First add to persistent data
        if (InventoryData.Instance != null)
        {
            InventoryData.Instance.AddItem(item);
        }

        // Then update UI
        return AddItemToUI(item, 1);
    }

    // Add item to UI only (used for loading and adding new items)
    private bool AddItemToUI(Item item, int countToAdd)
    {
        // Check if item already exists in UI and stack it
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                // Get the actual count from InventoryData
                var dataEntry = InventoryData.Instance?.inventory.Find(entry => entry.item == item);
                if (dataEntry != null)
                {
                    itemInSlot.count = dataEntry.count;
                    itemInSlot.RefreshCount();
                }
                return true;
            }
        }

        // Find empty slot for new item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot, countToAdd);
                return true;
            }
        }

        return false;
    }

    // Now accepts count parameter
    void SpawnNewItem(Item item, InventorySlot slot, int count = 1)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item, count);
    }

    public int CountItem(ItemType type)
    {
        // Get count from persistent data instead of UI
        if (InventoryData.Instance != null)
        {
            return InventoryData.Instance.CountItem(type);
        }

        // Fallback to UI count if no persistent data
        int total = 0;
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item.type == type)
            {
                total += itemInSlot.count;
            }
        }
        return total;
    }

    public void ShowItemDetails(Item item)
    {
        if (item != null)
        {
            itemNameText.text = item.itemName;
            itemDescriptionText.text = item.description;
            itemDetailImage.sprite = item.image;

            if (itemImageContainer != null)
            {
                itemImageContainer.SetActive(true);
            }
        }
    }

    public void ClearItemDetails()
    {
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemDetailImage.sprite = null;

        if (itemImageContainer != null)
        {
            itemImageContainer.SetActive(false);
        }
    }

    public void TriggerPickupAnimation()
    {
        if (inventoryCanvasAnimator != null)
        {
            inventoryCanvasAnimator.SetTrigger("ItemPickup");
        }
        else
        {
            Debug.LogWarning("InventoryManager: inventoryCanvasAnimator is not assigned!");
        }
    }

    // Call this to refresh UI when items are added from other sources
    public void RefreshInventoryUI()
    {
        LoadInventoryFromData();
    }
}