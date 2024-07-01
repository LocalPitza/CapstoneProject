using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    SeedData seedToGrow;

    [Header("Life Stage")]
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

    //Called when the Player plants a Seed
    public void Plant(SeedData seedToGorw)
    {
        this.seedToGrow = seedToGorw;

        //Harvestable
        seedling = Instantiate(seedToGorw.seedling, transform);

        ItemData croptToYield = seedToGorw.cropToYield;

        harvestable = Instantiate(croptToYield.harvestableModel, transform);

        maxGrowth = DayCounter.Instance.GetCurrentDay() + seedToGrow.daysToGrow;

        SwitchState(CropState.Seed);
    }

    public void Grow()
    {
        growth++;

        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        if(growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    void SwitchState(CropState stateToSwitch)
    {
        //Resets everything and set all Gameojects to fales
        seed.SetActive(false);
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

                harvestable.transform.parent = null;
                break;
        }

        cropState = stateToSwitch;
    }
}
