using UnityEngine;
using Cinemachine;
using TMPro;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public Canvas potGuideUI; // Assign your Canvas in the Inspector
    public CinemachineVirtualCamera targetCamera;
    public TMP_Text guideText;
    public TMP_Text plantInformtation;

    private Transform cameraTransform; // To store the camera's transform

    public int id;

    public enum SoilStatus
    {
        Soil, Digged, Watered, Weeds
    }

    public SoilStatus soilStatus;

    public Material soilMat, diggedMat, wateredMat, weedMat;
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

        // Update the text UI
        UpdatePotGuideText();
        UpdatePlantInformation();
    }

    private void UpdatePlantInformation()
    {
        if (plantInformtation == null) return;

        if (cropPlanted != null)
        {
            string plantName = cropPlanted.seedData != null ? cropPlanted.seedData.name.Replace(" Seed", "") : "Unknown";

            // Default text for the plant
            plantInformtation.text = $"{plantName}";

            if (cropPlanted.cropState == NewCropBehaviour.CropState.Wilted)
            {
                plantInformtation.text = "Dead Plant";
            }
            else if (soilStatus == SoilStatus.Watered)
            {
                // Show "Days Left to Harvest" when watered
                int daysLeft = cropPlanted.GetDaysLeftToHarvest();
                plantInformtation.text += $"\nDays Left to Harvest: {daysLeft}";
                plantInformtation.text += "\nGrowing!";
            }
            else
            {
                // Show "Days Left to Wither" instead when NOT watered
                if (cropPlanted.cropState != NewCropBehaviour.CropState.Seed) // Only for seedling or later
                {
                    int daysLeftToWither = cropPlanted.GetDaysLeftToWither();
                    plantInformtation.text += $"\nDays Left to Wither: {Mathf.Max(daysLeftToWither, 0)}";
                }
                plantInformtation.text += "\nNot Growing!";
            }

            plantInformtation.transform.parent.gameObject.SetActive(true); // Enable UI
        }
        else
        {
            plantInformtation.text = "";
            plantInformtation.transform.parent.gameObject.SetActive(false); // Disable UI when no plant
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
        // Null check to avoid errors
        if (renderer != null && materialToSwitch != null)
        {
            renderer.material = materialToSwitch;
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

    private void UpdatePotGuideText()
    {
        if (guideText == null) return;

        if (soilStatus == SoilStatus.Weeds)
        {
            guideText.text = "Hand Trowel to Remove Weeds";
            return;
        }

        if (cropPlanted != null)
        {
            if (cropPlanted.cropState == NewCropBehaviour.CropState.Harvestable)
            {
                guideText.text = "Glove to Harvest";
                return;
            }
            else if (cropPlanted.cropState == NewCropBehaviour.CropState.Wilted)
            {
                guideText.text = "Hoe to Remove Plant";
                return;
            }
        }

        switch (soilStatus)
        {
            case SoilStatus.Soil:
                guideText.text = "Hand Trowel to Dig";
                break;
            case SoilStatus.Digged:
                guideText.text = (cropPlanted != null) ? "Needs Water" : "Ready to Plant";
                break;
            case SoilStatus.Watered:
                guideText.text = "Watered";
                break;
        }
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
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
                    SwitchSoilStatus(SoilStatus.Soil); // Remove weeds
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

        UpdatePlantInformation();
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

        UpdatePlantInformation();
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisterTracker(this);
    }
}
