using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public enum SoilStatus
    {
        Soil, Digged, Watered
    }

    public SoilStatus soilStatus;
    private SoilStatus previousStatus;

    public Material soilMat, diggedMat, wateredMat;
    new Renderer renderer;

    public GameObject select;

    GameTimeStamp timeWatered;

    [Header("Crop")]
    public GameObject cropPrefab;
    public Transform plantPosition;
    NewCropBehaviour cropPlanted = null;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default Material
        SwitchSoilStatus(SoilStatus.Soil);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchSoilStatus(SoilStatus statusToSwitch)
    {
        if (statusToSwitch == SoilStatus.Watered)
        {
            previousStatus = soilStatus;
            timeWatered = TimeManager.Instance.GetGameTimeStamp();
        }

        soilStatus = statusToSwitch;

        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case SoilStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case SoilStatus.Digged:
                materialToSwitch = diggedMat;

                break;
            case SoilStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimeStamp();
                break;
        }
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);

        if (toggle)
        {
            SoilManager.Instance.SetSelectedSoil(this);
        }
    }

    public void Interact()
    {
        ItemData playerToolSlot = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.PlayerTool);
        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        if (playerToolSlot == null) 
        {
            return;
        }

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    if (soilStatus != SoilStatus.Digged)
                    {
                        SwitchSoilStatus(SoilStatus.Digged);
                    }
                    break;

                case EquipmentData.ToolType.WateringCan:
                    if (soilStatus != SoilStatus.Watered)
                    {
                        SwitchSoilStatus(SoilStatus.Watered);
                    }
                    break;

                case EquipmentData.ToolType.Hoe:
                    //Remove the Plant from the Soil
                    if(cropPlanted != null)
                    {
                        Destroy(cropPlanted.gameObject);
                        SwitchSoilStatus(SoilStatus.Soil);
                    }

                    break;
            }

            return;
        }
    }

    public NewCropBehaviour.CropState GetCropStatus()
    {
        if (cropPlanted != null)
        {
            return cropPlanted.cropState;
        }
        return NewCropBehaviour.CropState.Harvestable;
    }

    public void PlantSeed()
    {
        ItemData selectSeed = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Seed);
        SeedData seed = selectSeed as SeedData;

        if (selectSeed == null)
        {
            return;
        }

        if (seed != null && soilStatus != SoilStatus.Soil && cropPlanted == null)
        {
            GameObject cropObject = Instantiate(cropPrefab, transform);
            cropObject.transform.position = plantPosition.position;

            cropPlanted = cropObject.GetComponent<NewCropBehaviour>();
            cropPlanted.Plant(seed);
        }
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if(soilStatus == SoilStatus.Watered)
        {
            int hoursElapsed = GameTimeStamp.CompareTimestamp(timeWatered, timestamp);

            //Plant grow if being watered
            if(cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if(hoursElapsed > 24)
            {
                SwitchSoilStatus(previousStatus);
            }
        }

        //Handle the wilting when the land is dry
        if(soilStatus != SoilStatus.Watered && cropPlanted != null)
        {
            //If the Crop has already germinated, start the withering
            if (cropPlanted.cropState != NewCropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }
}
