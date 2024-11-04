using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    public ItemSlotData(ItemData itemData)
    {
        this.itemData = itemData;
        quantity = 1;
        ValidateQuantity();
    }

    //Stacking
    public void AddQuantity()
    {
        AddQuantity(1);
    }

    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }

    //Called when the Player used an Item
    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }

    public void ValidateQuantity()
    {
        if(quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }

    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }
}
