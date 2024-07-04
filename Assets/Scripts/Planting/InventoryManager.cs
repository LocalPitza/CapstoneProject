using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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


    [Header("Tools")]
    // Tool Slots
    public ItemData[] tools = new ItemData[8];
    // Tool on Hand
    public ItemData equipedTool = null;

    [Header("Storage")]
    // Item Slots
    public ItemData[] items = new ItemData[32];
    // Item on Hand
    public ItemData equipedItem = null;


    //Equiping
    //Handles Movments of Items from Inventory to Hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            // Cache the Inventory slot ItemData from InventoryManager
            ItemData itemToEquip = items[slotIndex];

            // Change the Inventory Slot to the Hand
            items[slotIndex] = equipedItem;

            // Change the Hand's Slot to the Inventory Slot's
            equipedItem = itemToEquip;
        }
        else
        {
            // Cache the Inventory slot ItemData from InventoryManager
            ItemData toolToEquip = tools[slotIndex];

            // Change the Inventory Slot to the Hand
            tools[slotIndex] = equipedTool;

            // Change the Hand's Slot to the Inventory Slot's
            equipedTool = toolToEquip;
        }

        // Updates the Changes
        UIManager.Instance.RenderInventory();
    }

    //Hand to Inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = equipedItem;
                    equipedItem = null;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i] == null)
                {
                    tools[i] = equipedTool;
                    equipedTool = null;
                    break;
                }
            }
        }

        UIManager.Instance.RenderInventory();
    }
}
