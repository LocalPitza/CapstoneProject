using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public ItemData item;

    public UnityEvent onInteract = new UnityEvent();

    public virtual void PickUp()
    {
        onInteract?.Invoke();

        Debug.Log("Harvest Plant");
        //Move Item from Equipped to Inventory
        NewInventoryManager.Instance.EquipHandSlot(item);
        NewUIManager.Instance.RenderInventory();
        //NewInventoryManager.Instance.RenderHand();

        Destroy(gameObject);
    }
}
