using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    public ItemData item;

    public virtual void PickUp()
    {
        Debug.Log("Harvest Plant");
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.harvestProduct = item;
        //NewInventoryManager.Instance.RenderHand();

        Destroy(gameObject);
    }
}
