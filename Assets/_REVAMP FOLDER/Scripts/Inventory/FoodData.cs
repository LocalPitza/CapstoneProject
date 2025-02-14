using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]

public class FoodData : ItemData
{
    public int hungerRefill;
    public int energyRefill;
    public bool requirePayment;
    public bool needIngredient;
    public RequiredIngredient[] requiredIngredients;

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
