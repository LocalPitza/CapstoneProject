using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public ItemData item;

    public virtual void Harvest()
    {
        // Ensure the previous item is moved to the inventory if equipped
        if (InventoryManager.Instance.equippedItem != null)
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
        }

        // Assign the new item to the equippedItem slot
        InventoryManager.Instance.equippedItem = item;

        Destroy(gameObject);
    }
}
