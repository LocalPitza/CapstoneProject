using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewHandInventorySlot : NewInventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Move Item from Equipped to Inventory
        //NewInventoryManager.Instance.EquipToInventory(inventoryType);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Left-click: Move the entire stack from equipped to inventory
            NewInventoryManager.Instance.EquipToInventory(inventoryType);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Right-click: Unstack one item and move it to the inventory
            NewInventoryManager.Instance.UnstackEquippedItem(inventoryType);
        }
    }
}
