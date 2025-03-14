using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]

public class FoodData : ItemData
{
    public int hungerRefill;
    public int energyRefill;
    public bool requirePayment;
    public bool needIngredient;
    public RequiredIngredient[] requiredIngredients;

    private void OnValidate()
    {
        UpdateDescription();
    }

    private void UpdateDescription()
    {
        // **Reset NewDescription every time OnValidate() runs** to avoid duplication
        NewDescription = description;

        // Append Hunger and Energy Refill values
        NewDescription += $"\n\nHunger Refill: {hungerRefill}\nEnergy Refill: {energyRefill}";

        // Append required ingredients if needed
        if (needIngredient && requiredIngredients.Length > 0)
        {
            NewDescription += "\nRequired Ingredients:";
            foreach (var ingredient in requiredIngredients)
            {
                if (ingredient.ingredient != null) // Ensure ingredient is not null
                    NewDescription += $"\n- {ingredient.ingredient.name} x{ingredient.requiredAmount}";
            }
        }
    }

    public void OnConsume()
    {
        PlayerStats.HungerStat(hungerRefill);
        PlayerStats.RestoreStamina(energyRefill);
    }

    [System.Serializable]
    public class RequiredIngredient
    {
        public ItemData ingredient;  // The ingredient needed
        public int requiredAmount;   // How many are needed
    }
}
