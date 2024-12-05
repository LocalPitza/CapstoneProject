using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewHandInventorySlot : NewInventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.EquipToInventory(inventoryType);
    }
}
