using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookManager : MonoBehaviour
{
    public static CookManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool ConsumeIngredients(FoodData.RequiredIngredient[] requiredIngredients, int quantity)
    {
        Dictionary<ItemData, int> requiredCounts = new Dictionary<ItemData, int>();

        // Calculate total required per ingredient
        foreach (FoodData.RequiredIngredient req in requiredIngredients)
        {
            int totalRequired = req.requiredAmount * quantity;
            requiredCounts[req.ingredient] = totalRequired;
        }

        // Check if all ingredients are available
        Dictionary<int, int> slotsToConsume = new Dictionary<int, int>(); // slot index -> amount to consume

        ItemSlotData[] harvestedSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest);

        foreach (var required in requiredCounts)
        {
            int remainingNeeded = required.Value;

            for (int i = 0; i < harvestedSlots.Length; i++)
            {
                if (harvestedSlots[i].itemData == required.Key)
                {
                    int available = harvestedSlots[i].quantity;

                    if (available >= remainingNeeded)
                    {
                        slotsToConsume[i] = remainingNeeded;
                        remainingNeeded = 0;
                        break;
                    }
                    else
                    {
                        slotsToConsume[i] = available;
                        remainingNeeded -= available;
                    }
                }
            }

            if (remainingNeeded > 0) return false; // Not enough ingredients
        }

        // Deduct ingredients from inventory
        foreach (var slot in slotsToConsume)
        {
            harvestedSlots[slot.Key].quantity -= slot.Value;
            if (harvestedSlots[slot.Key].quantity <= 0) harvestedSlots[slot.Key].Empty();
        }

        NewUIManager.Instance.RenderInventory();
        return true;
    }

    public bool HasEnoughIngredients(FoodData.RequiredIngredient[] requiredIngredients, int quantity)
    {
        Dictionary<ItemData, int> requiredCounts = new Dictionary<ItemData, int>();

        // Calculate total required per ingredient
        foreach (FoodData.RequiredIngredient req in requiredIngredients)
        {
            int totalRequired = req.requiredAmount * quantity;
            requiredCounts[req.ingredient] = totalRequired;
        }

        // Check if all ingredients are available
        foreach (var required in requiredCounts)
        {
            int remainingNeeded = required.Value;
            foreach (ItemSlotData slot in NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest))
            {
                if (slot.itemData == required.Key)
                {
                    remainingNeeded -= slot.quantity;
                    if (remainingNeeded <= 0) break;
                }
            }

            if (remainingNeeded > 0) return false; // Not enough ingredients
        }

        return true; // All ingredients are available
    }
}
