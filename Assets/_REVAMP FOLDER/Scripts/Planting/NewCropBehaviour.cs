using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCropBehaviour : MonoBehaviour
{
    int soilID;

    SeedData seedToGrow;

    [Header("Plant Stages")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    int growth;
    int maxGrowth;

    int maxHealth = GameTimeStamp.HoursToMinutes(48);
    int health;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }
    public CropState cropState;

    public SeedData seedData => seedToGrow;

    public int GetDaysLeftToHarvest()
    {
        if (seedToGrow == null) return 0; // Safety check
        int daysLeft = Mathf.CeilToInt((maxGrowth - growth) / (float)GameTimeStamp.HoursToMinutes(24));
        return Mathf.Max(daysLeft, 0);
    }

    //Initialisation for the crop GameObject
    //Called when the player plants a seed
    public void Plant(int soilID, SeedData seedToGrow)
    {
        LoadCrop(soilID, seedToGrow, CropState.Seed, 0, 0);

        SoilManager.Instance.RegisterCrop(soilID, seedToGrow, cropState, growth, health);
    }

    public void LoadCrop(int soilID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.soilID = soilID;

        this.seedToGrow = seedToGrow;

        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Days to Grow into Hours
        int hoursToGrow = GameTimeStamp.DaysToHours(seedToGrow.daysToGrow);
        //Convert to Minutes, since tha plant grows by the minutes
        maxGrowth = GameTimeStamp.HoursToMinutes(hoursToGrow);

        this.growth = growth;
        this.health = health;

        //Check if its regrowable
        if (seedToGrow.regrowable)
        {
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();
            regrowableHarvest.SetParent(this);
        }

        //Initial state to seed
        SwitchState(cropState);
    }

    public void Grow()
    {
        growth++;

        if(health < maxHealth)
        {
            health++;
        }

        //The seed will sprout into a seedling when growth is at 50%
        if(growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Grow from seedling to harvestable
        if (growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }

        SoilManager.Instance.OnCropStateChange(soilID, cropState, growth, health);
    }

    public void Wither()
    {
        health--;

        if (health <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }

        SoilManager.Instance.OnCropStateChange(soilID, cropState, growth, health);
    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(true);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);

                if (this.health <= 0) //Only reset health if it's empty
                {
                    health = maxHealth;
                }

                //health = maxHealth;

                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);

                //If the seed is not regrowable, detach the harvestable from this gameobject and destroy it.
                if (!seedToGrow.regrowable)
                {
                    //Unparenting
                    harvestable.transform.parent = null;

                    //RemoveCrop();

                    harvestable.GetComponent<InteractableObject>().onInteract.AddListener(RemoveCrop);
                }
                
                break;
            case CropState.Wilted:
                wilted.SetActive(true);
                break;
        }
        cropState = stateToSwitch;
    }

    public void RemoveCrop()
    {
        SoilManager.Instance.DeregisterCrop(soilID);
        Destroy(gameObject);
    }

    public void Regrow()
    {
        //Reset Growth
        //Get the regrowth time in hours
        int hoursToRegrow = GameTimeStamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimeStamp.HoursToMinutes(hoursToRegrow);
        SwitchState(CropState.Seedling);
    }

    public int GetDaysLeftToWither()
    {
        return Mathf.CeilToInt(health / (float)GameTimeStamp.HoursToMinutes(24));
    }
}
