using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandInventorySlot : InventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Moves Item from the Hand into Inventory
        InventoryManager.Instance.HandToInventory(inventoryType);
    }
}
