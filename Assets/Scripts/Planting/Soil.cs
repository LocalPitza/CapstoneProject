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

    //private bool isInteracting = false;

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
                //Debug.Log("Digged");
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
        //if (isInteracting) return;
        //isInteracting = true;

        ItemData toolSlot = InventoryManager.Instance.equipedTool;
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
            }
        }

        SeedData seedTool = toolSlot as SeedData;

        if (seedTool != null && soilStatus != SoilStatus.Dry && cropPlanted == null)
        {
            GameObject cropObject = Instantiate(cropPrefab, transform);

            // Moving the Crop Object to the Top of the Soil
            cropObject.transform.localPosition = new Vector3(0, 0.5f, 0);

            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.Plant(seedTool);
        }

        //isInteracting = false;
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
}


/*if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;
            //Debug.Log("Tool Type: " + toolType);

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:

                    //int totalSoilCount = ListOfSoil.allSoils.Count; // Get the total number of soils
                    //int adjustedEnergyCost = plantingEnergyCost - totalSoilCount; // Adjust the energy cost

                    //EnergyBar.Instance.DeductEnergy(adjustedEnergyCost); // Deduct the adjusted energy cost
                    //Debug.Log(adjustedEnergyCost);

                    SwitchLandStatus(SoilStatus.Digged);
                    energyBar.DeductEnergy(10);
                    break;

                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(SoilStatus.Watered);
                    break;
            }

            //Debug.Log("Interaction successful with tool: " + toolType);
            return;
        }

        /*Debug.Log("Selected Soil: " + (ListOfSoil.selectedSoil != null ? ListOfSoil.selectedSoil.GetInstanceID().ToString() : "null") + " | Current Soil: " + this.GetInstanceID());

        /if (ListOfSoil.selectedSoil != null && ListOfSoil.selectedSoil == this) // Check if this is the selected soil
        {
            if (equipmentTool != null)
            {
                EquipmentData.ToolType toolType = equipmentTool.toolType;
                Debug.Log("Tool Type: " + toolType);

                switch (toolType)
                {
                    case EquipmentData.ToolType.HandTrowel:
                        EnergyBar.Instance.DeductEnergy(plantingEnergyCost); // Deduct energy from EnergyBar
                        SwitchLandStatus(SoilStatus.Digged);
                        break;

                    case EquipmentData.ToolType.WateringCan:
                        SwitchLandStatus(SoilStatus.Watered);
                        break;
                }

                Debug.Log("Interaction successful with tool: " + toolType);
                return;
            }
        }
        else
        {
            Debug.Log("Not selected soil. Interaction skipped.");
        }*/