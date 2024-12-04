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
        Debug.Log("Harvest Regrowable");
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.harvestProduct = item;
        //NewInventoryManager.Instance.RenderHand();

        parentCrop.Regrow();
    }
}
