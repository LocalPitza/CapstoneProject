using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public Canvas potGuideUI; // Assign your Canvas in the Inspector
    public CinemachineVirtualCamera targetCamera;
    public GameObject materialTarget;

    [Header("Status Icons")]
    public Image statusIcon;
    public Sprite noWaterIcon;
    public Sprite weedIcon;

    private Transform cameraTransform; // To store the camera's transform

    public int id;

    public enum SoilStatus
    {
        Soil, Digged, Watered, Weeds
    }

    public SoilStatus soilStatus;

    public Material soilMat, diggedMat, wateredMat, weedMat;
    //new Renderer renderer;

    public GameObject select;

    GameTimeStamp timeWatered;

    [Header("Crop")]
    public GameObject cropPrefab;
    public Transform plantPosition;
    NewCropBehaviour cropPlanted = null;

    void Start()
    {
        // Get the camera transform from the assigned Cinemachine Virtual Camera
        if (targetCamera != null)
        {
            cameraTransform = targetCamera.transform;
        }
        else
        {
            Debug.LogError("No Cinemachine Virtual Camera assigned to PottingSoil!");
        }

        //Default Material
        SwitchSoilStatus(SoilStatus.Soil);

        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }


    void LateUpdate()
    {
        if (potGuideUI != null && cameraTransform != null)
        {
            // Make the Canvas always face the Cinemachine Camera
            potGuideUI.transform.LookAt(potGuideUI.transform.position + cameraTransform.forward);
        }

        if (select.activeSelf)
        {
            UpdatePlayerUI();
        }

        UpdateSoilStatusIcon();
    }

    void UpdateSoilStatusIcon()
    {
        // Handle status icons
        if (soilStatus == SoilStatus.Weeds)
        {
            statusIcon.sprite = weedIcon;
            statusIcon.enabled = true;
        }
        else if (cropPlanted != null && soilStatus != SoilStatus.Watered)
        {
            statusIcon.sprite = noWaterIcon;
            statusIcon.enabled = true;
        }
        else
        {
            statusIcon.enabled = false;
        }
    }

    private void UpdatePlayerUI()
    {
        List<string> guideLines = new List<string>();
        string plantInfo = "";

        // --- Guide Text ---
        if (cropPlanted != null)
        {
            if (cropPlanted.cropState == NewCropBehaviour.CropState.Harvestable)
            {
                // If the plant is ready to harvest and the soil is in weeds state, show "Remove Weeds First"
                if (soilStatus == SoilStatus.Weeds)
                {
                    plantInfo = $"{cropPlanted.seedData.name.Replace(" Seed", "")}\nReady to Harvest";
                    guideLines.Add("Remove Weeds First");
                }
                else
                {
                    // If the plant is ready to harvest, show only relevant information (removed the "Growing" or "Not Growing")
                    plantInfo = $"{cropPlanted.seedData.name.Replace(" Seed", "")}\nReady to Harvest";
                    guideLines.Add("Use Glove and press F to harvest");

                    // Also add "Days to Wither" if it's harvestable
                    int daysLeftToWither = cropPlanted.GetDaysLeftToWither();
                    plantInfo += $"\nDays Left to Wither: {daysLeftToWither}";
                }
            }
            else if (cropPlanted.cropState == NewCropBehaviour.CropState.Wilted)
            {
                // If the plant is dead (wilted), show only these two messages
                plantInfo = "Dead Plant";
                guideLines.Add("Hoe to Remove");
            }
            else if (soilStatus == SoilStatus.Weeds)
            {
                // If soil is in weeds state and there's no plant, show "Remove Weed First"
                guideLines.Add("Remove Weeds First");
            }
            else
            {
                // Other conditions when the plant is present
                if (soilStatus == SoilStatus.Watered)
                {
                    int daysLeft = cropPlanted.GetDaysLeftToHarvest();
                    plantInfo = $"{cropPlanted.seedData.name.Replace(" Seed", "")}\nGrowing!\nDays Left to Harvest: {daysLeft}";
                }
                else
                {
                    if (cropPlanted.cropState != NewCropBehaviour.CropState.Seed)
                    {
                        int daysLeftToWither = cropPlanted.GetDaysLeftToWither();
                        plantInfo = $"{cropPlanted.seedData.name.Replace(" Seed", "")}\nNot Growing!\nDays Left to Wither: {Mathf.Max(daysLeftToWither, 0)}";
                    }
                    else
                    {
                        plantInfo = $"{cropPlanted.seedData.name.Replace(" Seed", "")}\nNot Growing!";
                    }

                    // Add "Needs Water" if necessary, but exclude if plant is ready to harvest
                    if (soilStatus != SoilStatus.Weeds && soilStatus != SoilStatus.Watered && cropPlanted.cropState != NewCropBehaviour.CropState.Harvestable)
                    {
                        plantInfo += "\nNeeds Water";
                    }
                }
            }
        }
        else
        {
            // If there's no plant, handle soil status accordingly
            if (soilStatus == SoilStatus.Soil)
            {
                guideLines.Add("Hand Trowel to Dig");
            }
            else if (soilStatus == SoilStatus.Digged)
            {
                guideLines.Add("Ready to Plant");
            }
            else if (soilStatus == SoilStatus.Watered)
            {
                // If soil is watered and there's no plant, show "Watered"
                guideLines.Add("Watered");
            }
            else if (soilStatus == SoilStatus.Weeds)
            {
                // If there's no plant and soil is in Weeds, show "Remove Weeds First"
                guideLines.Add("Remove Weeds First");
            }
        }

        // Combine the guide text and plant info into one string
        string fullText = plantInfo + "\n" + string.Join("\n", guideLines);

        // Show or hide the UI based on whether there’s content
        if (!string.IsNullOrEmpty(fullText))
        {
            PlantStatus.Instance?.ShowStatus(fullText); // Show the combined text
        }
        else
        {
            PlantStatus.Instance?.HideStatus();
        }
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

            case SoilStatus.Weeds:
                materialToSwitch = weedMat;
                break;
        }

        // Change material on the assigned GameObject
        Renderer targetRenderer = materialTarget != null ? materialTarget.GetComponent<Renderer>() : null;

        if (targetRenderer != null && materialToSwitch != null)
        {
            targetRenderer.material = materialToSwitch;
        }
        else
        {
            Debug.Log($"Renderer or material is null on {gameObject.name}. Skipping material update.");
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

            case SoilStatus.Weeds:
                materialToSwitch = weedMat;
                timeWatered = null; // Reset water timestamp
                break;

        }

        // Change material on the assigned GameObject
        Renderer targetRenderer = materialTarget != null ? materialTarget.GetComponent<Renderer>() : null;

        if (targetRenderer != null && materialToSwitch != null)
        {
            targetRenderer.material = materialToSwitch;
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

        if (!toggle)
        {
            PlantStatus.Instance.HideStatus();
        }
    }

    public void Interact()
    {
        ItemData playerToolSlot = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Storage);

        //If there's nothing equipped, return
        if (!NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Storage)) 
        {
            return;
        }

        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        // If the soil is in the Weeds state, restrict interactions except removing weeds or removing plants
        if (soilStatus == SoilStatus.Weeds)
        {
            if (equipmentTool != null)
            {
                if (equipmentTool.toolType == EquipmentData.ToolType.HandTrowel)
                {
                    SwitchSoilStatus(SoilStatus.Digged); // Remove weeds
                    return;
                }
                else if (equipmentTool.toolType == EquipmentData.ToolType.Hoe && cropPlanted != null)
                {
                    Debug.Log("Remove Deadplant");
                    cropPlanted.RemoveCrop(); // Allow removing a plant even if there are weeds
                    return;
                }
            }
            return;
        }

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    if (soilStatus == SoilStatus.Soil)
                    {
                        SwitchSoilStatus(SoilStatus.Digged);
                    }
                    break;

                case EquipmentData.ToolType.WateringCan:
                    if (soilStatus == SoilStatus.Digged) // Ensure water only applies when digged
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

        if (seed != null)
        {
            if (soilStatus != SoilStatus.Soil && cropPlanted == null)
            {
                SpawnCrop();
                cropPlanted.Plant(id, seed);

                //Consumes the Item for planting
                NewInventoryManager.Instance.ConsumeItem
                    (NewInventoryManager.Instance.GetEquippedSlot
                    (NewInventorySlot.InventoryType.Storage));
            }
        }
    }

    public NewCropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);
        cropObject.transform.position = plantPosition.position;
        cropPlanted = cropObject.GetComponent<NewCropBehaviour>();

        //UpdatePlantInformation();
        UpdateSoilStatusIcon();
        return cropPlanted;
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if (soilStatus == SoilStatus.Watered)
        {
            int hoursElapsed = GameTimeStamp.CompareTimestamp(timeWatered, timestamp);

            // Plant grows if watered
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            // Reset to Digged if watered for more than 23 hours
            if (hoursElapsed > 23)
            {
                SwitchSoilStatus(SoilStatus.Digged);
            }
        }
        else if (soilStatus == SoilStatus.Digged)
        {
            // Handle weeds based on elapsed time since last watered
            if (timeWatered != null)
            {
                int hoursSinceWatered = GameTimeStamp.CompareTimestamp(timeWatered, timestamp);

                // Change to Weeds if more than 24 hours since last watered
                if (hoursSinceWatered > 24)
                {
                    SwitchSoilStatus(SoilStatus.Weeds);
                }
            }
        }

        //Handle the wilting when the land is dry
        if (soilStatus != SoilStatus.Watered && cropPlanted != null)
        {
            //If the Crop has already germinated, start the withering
            if (cropPlanted.cropState != NewCropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }

        //UpdatePlantInformation();
        UpdateSoilStatusIcon();
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisterTracker(this);
    }
}
