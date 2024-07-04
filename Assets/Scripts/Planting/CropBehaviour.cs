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

    private int daysSinceLastHarvest;
    private bool isHarvested;

    public enum CropState
    {
        Seed, Seedling, Harvestable
    }
    public CropState cropState;

    void Start()
    {
        DayCounter.Instance.OnDayAdvanced += OnDayAdvanced;
        daysSinceLastHarvest = 0;
        isHarvested = false;
    }

    void OnDestroy()
    {
        DayCounter.Instance.OnDayAdvanced -= OnDayAdvanced;
    }

    private void OnDayAdvanced()
    {
        if (isHarvested && seedToGrow.regrowable)
        {
            daysSinceLastHarvest++;
            if (daysSinceLastHarvest >= seedToGrow.daysToRegrow)
            {
                Regrow();
            }
        }
        else if (!isHarvested)
        {
            Grow();
        }
    }

    //Called when the Player plants a Seed
    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;

        // Seedling
        seedling = Instantiate(seedToGrow.seedling, transform);
        seedling.SetActive(false);

        // Harvestable
        ItemData cropToYield = seedToGrow.cropToYield;
        harvestable = Instantiate(cropToYield.fruitModel, transform);
        harvestable.SetActive(false);

        maxGrowth = DayCounter.Instance.GetCurrentDay() + seedToGrow.daysToGrow;

        //Check if Regrowable
        if (seedToGrow.regrowable)
        {
            RegrowablePlant regrowablePlant = harvestable.GetComponent<RegrowablePlant>();

            regrowablePlant.SetParent(this);
        }

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

    public void SwitchState(CropState stateToSwitch)
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

                if (!seedToGrow.regrowable)
                {
                    harvestable.transform.parent = null;
                    Destroy(gameObject);
                }
                break;
        }

        cropState = stateToSwitch;
    }

    public void Regrow()
    {
        daysSinceLastHarvest = 0;
        isHarvested = false;
        SwitchState(CropState.Seedling);
    }
}
