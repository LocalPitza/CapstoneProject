using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowablePlant : Harvestable
{
    CropBehaviour parentCrop;

    public void SetParent(CropBehaviour parentCrop)
    {
        this.parentCrop = parentCrop;
    }

    public override void Harvest()
    {
        // Ensure the previous item is moved to the inventory if equipped
        if (InventoryManager.Instance.equippedItem != null)
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
        }

        // Assign the new item to the equippedItem slot
        InventoryManager.Instance.equippedItem = item;

        parentCrop.Regrow();
    }
}
