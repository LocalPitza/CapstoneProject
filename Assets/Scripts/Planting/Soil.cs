using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    [Header("Energy Bar")]
    [SerializeField] EnergyBar energyBar;
    int plantingEnergyCost;

    public enum SoilStatus
    {
        Dry, Digged, Watered
    }

    public SoilStatus soilStatus;
    public Material soilMat, wateredMat;
    public GameObject digged;
    new Renderer renderer;

    private int lastWateredDay;
    public int daysUntilDry;

    [Header("Crops")]
    public GameObject cropPrefab;
    CropBehaviour cropPlanted = null;

    public GameObject indicator;


    private void Start()
    {
        renderer = GetComponent<Renderer>();

        if(EnergyBar.Instance != null)
        {
            plantingEnergyCost = EnergyBar.Instance.ammountPlantEnergy;
        }
        else
        {
            Debug.LogError("Energy Instance is Missing");
        }

        SwitchLandStatus(SoilStatus.Dry);

        DayCounter.Instance.OnDayAdvanced += CheckIfSoilShouldDry;
    }

    private void OnDestroy()
    {
        DayCounter.Instance.OnDayAdvanced -= CheckIfSoilShouldDry;
    }

    public void SwitchLandStatus(SoilStatus statusSwicth)
    {
        soilStatus = statusSwicth;
        Material materialSwitch = soilMat;

        switch (statusSwicth)
        {
            case SoilStatus.Digged:
                digged.SetActive(true);

                break;

            case SoilStatus.Dry:
                materialSwitch = soilMat;
                break;

            case SoilStatus.Watered:
                materialSwitch = wateredMat;
                lastWateredDay = DayCounter.Instance.GetCurrentDay();
                break;
        }

        renderer.material = materialSwitch;
    }

    public void Interact()
    {
        Debug.Log("Interacted");
        //Equiping Tools
        ItemData toolSlot = InventoryManager.Instance.equippedTool;
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:

                    EnergyBar.Instance.DeductEnergy(plantingEnergyCost);
                    SwitchLandStatus(SoilStatus.Digged);
                    break;

                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(SoilStatus.Watered);
                    break;

                case EquipmentData.ToolType.PruningShears:
                    Debug.Log("Pruning Shears Used");

                    //Harvest
                    PlayerInteraction harvestable = FindObjectOfType<PlayerInteraction>();

                    if (harvestable != null)
                    {
                        Debug.Log("Select Harvestable");
                        harvestable.HarvestableInteract();
                    }
                    else
                    {
                        Debug.LogWarning("No Select Pot Script");
                    }
                    break;
            }
        }


        //Planting
        SeedData seedTool = toolSlot as SeedData;

        if (seedTool != null && soilStatus != SoilStatus.Dry && cropPlanted == null)
        {
            GameObject cropObject = Instantiate(cropPrefab, transform);

            // Moving the Crop Object to the Top of the Soil
            cropObject.transform.localPosition = new Vector3(0, 0.5f, 0);

            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.Plant(seedTool);
        }
    }

    private void CheckIfSoilShouldDry()
    {
        if (soilStatus == SoilStatus.Watered)
        {
            int currentDay = DayCounter.Instance.GetCurrentDay();
            if (currentDay - lastWateredDay >= daysUntilDry)
            {
                SwitchLandStatus(SoilStatus.Dry);
            }

            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }
        }
    }

    public void Select(bool toggle)
    {
        indicator.SetActive(toggle);
    }
}