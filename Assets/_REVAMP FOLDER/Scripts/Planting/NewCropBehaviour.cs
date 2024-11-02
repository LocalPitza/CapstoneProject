using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCropBehaviour : MonoBehaviour
{
    SeedData seedToGrow;

    [Header("Plant Stages")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    int growth;
    int maxGrowth;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }
    public CropState cropState;

    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;
        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Days to Grow into Hours
        int hoursToGrow = GameTimeStamp.DaysToHours(seedToGrow.daysToGrow);
        //Convert to Minutes, since tha plant grows by the minutes
        maxGrowth = GameTimeStamp.HoursToMinutes(hoursToGrow);

        //Check if its regrowable
        if (seedToGrow.regrowable)
        {
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();
            regrowableHarvest.SetParent(this);
        }

        //Initial state to seed
        SwitchState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;

        //The seed will sprout into a seedling when growth is at 50%
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Grow to seedling
        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(true);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);

                //If the seed is not regrowable, detach the harvestable from this gameobject and destroy it.
                if (!seedToGrow.regrowable)
                {
                    //Unparenting
                    harvestable.transform.parent = null;
                    Destroy(gameObject);
                }
                
                break;
        }
        cropState = stateToSwitch;
    }

    public void Regrow()
    {
        //Reset Growth
        //Get the regrowth time in hours
        int hoursToRegrow = GameTimeStamp.DaysToHours(seedToGrow.daysToGrow);
        growth = maxGrowth - GameTimeStamp.HoursToMinutes(hoursToRegrow);
        SwitchState(CropState.Seedling);
    }
}
