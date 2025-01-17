using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEat : MonoBehaviour
{
    public static List<ItemSlotData> harvestToEat = new List<ItemSlotData>();

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

            if (handSlotItem == null)
            {
                return;
            }

            NewUIManager.Instance.TriggerYesNoPrompt($"Do you want to eat {handSlotItem.name} ? ", Eatingtable);
        }
    }

    void Eatingtable()
    {
        //Get the ItemsSlotData of what the player is holding
        ItemSlotData handSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        int hungerRefill = handSlot.quantity * handSlot.itemData.hungerRefill;

        PlayerStats.HungerStat(hungerRefill);

        handSlot.Empty();

        foreach (ItemSlotData harvest in harvestToEat)
        {
            Debug.Log($"In the Eating table: {harvest.itemData.name} x {harvest.quantity}");
        }
    }
}
