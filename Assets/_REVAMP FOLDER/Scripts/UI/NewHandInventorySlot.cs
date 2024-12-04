using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewHandInventorySlot : NewInventorySlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        // Set active item type based on inventory type
        if (inventoryType == InventoryType.Seed)
        {
            NewInventoryManager.Instance.SetActiveItemType(NewInventoryManager.ActiveItemType.SelectedSeed);
        }
        else if (inventoryType == InventoryType.Storage && NewInventoryManager.Instance.harvestProduct != null)
        {
            NewInventoryManager.Instance.SetActiveItemType(NewInventoryManager.ActiveItemType.HarvestProduct);
        }
        else if (inventoryType == InventoryType.Storage)
        {
            NewInventoryManager.Instance.SetActiveItemType(NewInventoryManager.ActiveItemType.SelectedStorage);
        }

        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.EquipToInventory(inventoryType);
    }
}
