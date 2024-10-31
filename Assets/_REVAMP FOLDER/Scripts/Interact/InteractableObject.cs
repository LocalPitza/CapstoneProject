using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    public ItemData item;

    public void PickUp()
    {
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.selectedPocket = item;
        NewInventoryManager.Instance.RenderHand();

        Destroy(gameObject);
    }
}
