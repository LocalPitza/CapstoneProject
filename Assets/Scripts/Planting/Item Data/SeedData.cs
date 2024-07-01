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
}
