using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CropSaveState
{
    public int soilID;

    public string seedToGrow;
    public NewCropBehaviour.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int soilID, string seedToGrow, NewCropBehaviour.CropState cropState, int growth, int health)
    {
        this.soilID = soilID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }

    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData) NewInventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);

        int maxGrowth = GameTimeStamp.HoursToMinutes(GameTimeStamp.DaysToHours(seedInfo.daysToGrow));
        int maxHealth = GameTimeStamp.HoursToMinutes(48);


        if (health < maxHealth)
        {
            health++;
        }

        //The seed will sprout into a seedling when growth is at 50%
        if (growth >= maxGrowth / 2 && cropState == NewCropBehaviour.CropState.Seed)
        {
            cropState = NewCropBehaviour.CropState.Seedling;
        }

        //Grow to seedling
        if (growth >= maxGrowth && cropState ==  NewCropBehaviour.CropState.Seedling)
        {
            cropState = NewCropBehaviour.CropState.Harvestable;
        }
    }

    public void Wither()
    {
        health--;

        if (health <= 0 && cropState !=  NewCropBehaviour.CropState.Seed)
        {
            cropState = NewCropBehaviour.CropState.Wilted;
        }

    }
}
