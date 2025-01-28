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
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            ItemData handSlotItem = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Harvest);

            if (handSlotItem == null)
            {
                return;
            }

            NewUIManager.Instance.TriggerYesNoPrompt($"Do you want to consuume {handSlotItem.name} ? ", Eatingtable);
        }
    }

    void Eatingtable()
    {
        //Get the ItemsSlotData of what the player is holding
        ItemSlotData handSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        int hungerRefill = handSlot.quantity * handSlot.itemData.hungerRefill;
        int energyRefill = handSlot.quantity * handSlot.itemData.energyRefill;

        PlayerStats.HungerStat(hungerRefill);

        // Update Stamina
        if (energyRefill > 0)
        {
            PlayerStats.Stamina += energyRefill;
            if (PlayerStats.Stamina > 100)
            {
                PlayerStats.Stamina = 100; // Cap stamina at max value
            }
            NewUIManager.Instance.RenderPlayerStats();
        }

        handSlot.Empty();

        foreach (ItemSlotData harvest in harvestToEat)
        {
            Debug.Log($"In the Eating table: {harvest.itemData.name} x {harvest.quantity}");
        }
    }
}
