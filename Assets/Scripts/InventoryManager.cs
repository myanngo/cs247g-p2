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
    }

    public bool AddItem(Item item)
    {
        // check if exists and is in stack
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // if none we can just go to the empty one (lol rip performance)
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public int CountItem(ItemType type)
    {
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
}