using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : MonoBehaviour
{
    public static List<ItemSlotData> harvestToShip = new List<ItemSlotData>();

    private InteractMessage interactMessage;
    void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
    }

    void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(KeyCode.F))
        {
            ItemData handSlotItem = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Harvest);

            if(handSlotItem == null )
            {
                //If not holding anything, move on
                return;
            }

            NewUIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ? ", PlaceItemInShippingBin);
        }
    }

    void PlaceItemInShippingBin()
    {
        //Get the ItemsSlotData of what the player is holding
        ItemSlotData handSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        //If holding 2 items and cost is 10, 2 * 10 = 20 (MoneyEarned)
        int moneyEarned = handSlot.quantity * handSlot.itemData.cost;

        PlayerStats.Earn(moneyEarned);

        //Holding nothing
        handSlot.Empty();

        foreach(ItemSlotData harvest in harvestToShip)
        {
            Debug.Log($"In the shipping bin: {harvest.itemData.name} x {harvest.quantity}");
        }

    }
}
