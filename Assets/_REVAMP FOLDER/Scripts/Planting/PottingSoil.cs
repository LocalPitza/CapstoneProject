using UnityEngine;
using Cinemachine;
using TMPro;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public Canvas potGuideUI; // Assign your Canvas in the Inspector
    public CinemachineVirtualCamera targetCamera;
    public TMP_Text guideText;

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

        switch (soilStatus)
        {
            case SoilStatus.Soil:
                potGuideUI.gameObject.SetActive(true);
                guideText.text = "Hand Trowel to Dig";
                break;
            case SoilStatus.Digged:
                potGuideUI.gameObject.SetActive(true);
                guideText.text = (cropPlanted != null) ? "Needs Water" : "Ready to Plant";
                break;
            case SoilStatus.Watered:
                guideText.text = "";
                potGuideUI.gameObject.SetActive(false);
                break;
            case SoilStatus.Weeds:
                potGuideUI.gameObject.SetActive(true);
                guideText.text = "Hand Trowel to Remove Weed";
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

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    if (soilStatus == SoilStatus.Weeds)
                    {
                        // HandTrowel is used to remove weeds and turn soil back to Digged
                        SwitchSoilStatus(SoilStatus.Digged);
                    }
                    else if (soilStatus == SoilStatus.Soil)
                    {
                        // HandTrowel is used to turn Soil into Digged
                        SwitchSoilStatus(SoilStatus.Digged);
                    }
                    break;

                case EquipmentData.ToolType.WateringCan:
                    if (soilStatus == SoilStatus.Weeds)
                    {
                        //I willl make a UI for this to Notify the Player
                        Debug.Log("Cannot water soil with weeds. Remove weeds first!");
                    }
                    else if (soilStatus != SoilStatus.Soil) // Only water if soil is digged or already watered
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

            // Reset to Digged if watered for more than 22 hours
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
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnregisterTracker(this);
    }
}
