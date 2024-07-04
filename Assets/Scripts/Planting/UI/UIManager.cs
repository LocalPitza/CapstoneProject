using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Status Bar")]
    //Hunger

    //Toilet

    //Energy

    //Equiped Item
    public Image toolEquipSlot;
    public Image itemEquipSlot;

    [Header("Invetory UI")]
    //Inventory UI
    public GameObject inventoryPanel;

    [Header("Inventory System")]
    public HandInventorySlot toolHandSlot;
    public InventorySlot[] toolSlots;

    public HandInventorySlot itemHandSlot;
    public InventorySlot[] storageSlots;

    //Info Box
    [Header("Item Name Box")]
    public TextMeshProUGUI itemNameText;

    [Header("Item Description Box")]
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
    }

    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
        }

        for (int i = 0; i < storageSlots.Length; i++)
        {
            storageSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        // Render Tool Section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        // Render Item Section
        RenderInventoryPanel(inventoryItemSlots, storageSlots);

        // Render the Equipped slots
        toolHandSlot.Display(InventoryManager.Instance.equipedTool);
        itemHandSlot.Display(InventoryManager.Instance.equipedItem);

        ItemData equippedTool = InventoryManager.Instance.equipedTool;
        ItemData equippedItem = InventoryManager.Instance.equipedItem;

        // Checking if there is a Tool
        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;
            toolEquipSlot.gameObject.SetActive(true);
        }
        else
        {
            toolEquipSlot.gameObject.SetActive(false);
        }

        if (equippedItem != null)
        {
            itemEquipSlot.sprite = equippedItem.thumbnail;
            itemEquipSlot.gameObject.SetActive(true);
        }
        else
        {
            itemEquipSlot.gameObject.SetActive(false);
        }
    }

    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i < slots.Length)
            {
                uiSlots[i].Display(slots[i]);
            }
            else
            {
                uiSlots[i].Display(null); // or handle the case where there is no item
            }
        }
    }


    //This will called by the Button for opening the Item Box
    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    //Display Item name and Description
    public void DisplayItemInfo(ItemData data)
    {
        if(data == null)
        {
            itemNameText.text = " ";
            itemDescriptionText.text = " ";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }
}
