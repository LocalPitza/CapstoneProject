using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public int id;

    public enum SoilStatus
    {
        Soil, Digged, Watered
    }

    public SoilStatus soilStatus;

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

        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadSoilData(SoilStatus statusToSwitch, GameTimeStamp lastwatered)
    {
        soilStatus = statusToSwitch;
        timeWatered = lastwatered;

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
                break;
        }
        // Null check to avoid errors
        if (renderer != null && materialToSwitch != null)
        {
            renderer.material = materialToSwitch;
        }
        else
        {
            Debug.LogWarning($"Renderer or material is null on {gameObject.name}. Skipping material update.");
        }
    }

    public void SwitchSoilStatus(SoilStatus statusToSwitch)
    {
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
        // Null check to avoid errors
        if (renderer != null && materialToSwitch != null)
        {
            renderer.material = materialToSwitch;
        }
        else
        {
            Debug.LogWarning($"Renderer or material is null on {gameObject.name}. Skipping material update.");
        }

        SoilManager.Instance.OnSoilStateChange(id, soilStatus, timeWatered);
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {
        ItemData playerToolSlot = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Storage);

        if (!NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Storage)) 
        {
            return;
        }

        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    SwitchSoilStatus(SoilStatus.Digged);

                    break;

                case EquipmentData.ToolType.WateringCan:
                    if (soilStatus != SoilStatus.Soil)
                    {
                        SwitchSoilStatus(SoilStatus.Watered);
                    }
                    break;

                case EquipmentData.ToolType.Hoe:
                    //Remove the Plant from the Soil
                    if(cropPlanted != null)
                    {
                        Debug.Log("Remove Deadplant");
                        cropPlanted.RemoveCrop();
                        //SwitchSoilStatus(SoilStatus.Soil);
                    }

                    break;
            }

            return;
        }

        SeedData seed = playerToolSlot as SeedData;

        if (seed != null && soilStatus != SoilStatus.Soil && cropPlanted == null)
        {
            Debug.Log("Planting");

            SpawnCrop();

            cropPlanted.Plant(id, seed);

            //Consumes the Item for planting
            NewInventoryManager.Instance.ConsumeItem
                (NewInventoryManager.Instance.GetEquippedSlot
                (NewInventorySlot.InventoryType.Storage));
        }
    }

    public NewCropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);
        cropObject.transform.position = plantPosition.position;

        cropPlanted = cropObject.GetComponent<NewCropBehaviour>();

        return cropPlanted;
    }

    /*public NewCropBehaviour.CropState GetCropStatus()
    {
        if (cropPlanted != null)
        {
            return cropPlanted.cropState;
        }
        return NewCropBehaviour.CropState.Harvestable;
    }*/

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
                SwitchSoilStatus(SoilStatus.Digged);
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

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisterTracker(this);
    }
}
