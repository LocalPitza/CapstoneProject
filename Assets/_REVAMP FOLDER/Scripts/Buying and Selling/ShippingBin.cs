using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingBin : MonoBehaviour
{
    //public static int hoursToShip = 18;
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
                return;
            }

            NewUIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} ? ", PlaceItemInShippingBin);
        }
    }

    void PlaceItemInShippingBin()
    {
        //Get the ItemsSlotData of what the player is holding
        ItemSlotData handSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        //harvestToShip.Add(new ItemSlotData(handSlot));

        int moneyEarned = handSlot.quantity * handSlot.itemData.cost;

        PlayerStats.Earn(moneyEarned);

        handSlot.Empty();

        foreach(ItemSlotData harvest in harvestToShip)
        {
            Debug.Log($"In the shipping bin: {harvest.itemData.name} x {harvest.quantity}");
        }

    }

    /*public static void ShipItens()
    {
        //Total on how much the player should receive upon shipping the items
        int moneyToReceive = TallyItems(harvestToShip);

        //Convent the Items to Money
        PlayerStats.Earn(moneyToReceive);

        //Empty the Shipping Bin
        harvestToShip.Clear();

    }*/

    static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;

        foreach(ItemSlotData item in items)
        {
            total += item.quantity * item.itemData.cost;
        }

        return total;
    }
}
