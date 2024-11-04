using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowableHarvestBehaviour : InteractableObject
{
    NewCropBehaviour parentCrop;

    public void SetParent(NewCropBehaviour parentCrop)
    {
        this.parentCrop = parentCrop;
    }

    public override void PickUp()
    {
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.EquipEmptySlot(item);

        NewInventoryManager.Instance.RenderHand();

        parentCrop.Regrow();
    }
}
