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
    public InventorySlot[] itemSlots;

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
        for(int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render Tool Section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render Item Section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Renders the Equiped slots
        toolHandSlot.Display(InventoryManager.Instance.equipedTool);
        itemHandSlot.Display(InventoryManager.Instance.equipedItem);


        //Get Tool Equip from the Inventory Manager
        ItemData equippedTool = InventoryManager.Instance.equipedTool;
        ItemData equippedItem = InventoryManager.Instance.equipedItem;
        
        //Checking if there is a Tool
        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;
            toolEquipSlot.gameObject.SetActive(true);

            return;
        }

        toolEquipSlot.gameObject.SetActive(false);

        if(equippedItem != null)
        {
            itemEquipSlot.sprite = equippedItem.thumbnail;
            itemEquipSlot.gameObject.SetActive(true);

            return;
        }

        itemEquipSlot.gameObject.SetActive(false);
    }

    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
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
