using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
    //Time it takes for the seed matures into a crop
    public int daysToGrow;

    //The Crop the seeds will yield
    public ItemData cropToYield;

    //The Seedling Game Object
    public GameObject seedling;

    [Header("Regrowable")]
    public bool regrowable;
    public int daysToRegrow;

    private void OnValidate()
    {
        itemName = this.name;

        UpdateDescription();
    }

    private void UpdateDescription()
    {
        // Reset NewDescription before adding new details
        description = itemName;

        // Append Days to Harvest info
        description += $"\n\nDays to Harvest: {daysToGrow}";

        // Append Regrowable info
        string regrowableText = regrowable ? "Yes" : "No";
        description += $"\nRegrowable: {regrowableText}";

        // Append Days to Regrow if applicable
        if (regrowable)
        {
            description += $"\nDays to Regrow: {daysToRegrow}";
        }
    }
}
