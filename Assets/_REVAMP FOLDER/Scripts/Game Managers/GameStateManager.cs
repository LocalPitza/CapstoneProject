using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    public bool IsFading { get; private set; } = false;

    public Image fadeImage;
    public float fadeDuration = 1.0f;

    public bool debugging = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        TimeManager.Instance.RegisterTracker(this);

        // Ensure fade image is active and fully black at the start
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        // Start fading to transparent
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (debugging)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Sleep();
            }

            if (Input.GetKey(KeyCode.U))
            {
                TimeManager.Instance.Tick();
            }
        }
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        UpdateFarmState(timestamp);
    }

    void UpdateFarmState(GameTimeStamp timestamp)
    {
        //Updates the Land and Crop Save states as long as the player is outside of the PlantingArea scene
        if(SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.MainGame)
        {
            if (SoilManager.urbanFarmData == null)
            {
                return;
            }

            List<SoilSaveState> soilData = SoilManager.urbanFarmData.Item1;
            List<CropSaveState> cropData = SoilManager.urbanFarmData.Item2;

            //If there are no crops planted on a certain soil, don't need to update
            if(cropData.Count == 0)
            {
                return;
            }

            for(int i = 0; i < cropData.Count; i++)
            {
                CropSaveState crop = cropData[i];
                SoilSaveState soil = soilData[crop.soilID];

                //Check if the crop is already wilted
                if(crop.cropState == NewCropBehaviour.CropState.Wilted)
                {
                    continue;
                }

                //Update the Soil's state
                soil.ClockUpdate(timestamp);

                //Update the crop's state based on the soil state
                if(soil.soilStatus == PottingSoil.SoilStatus.Watered)
                {
                    crop.Grow();
                }
                else if(crop.cropState != NewCropBehaviour.CropState.Seed)
                {
                    crop.Wither();
                }

                //Update the element in the array
                cropData[i] = crop;
                soilData[crop.soilID] = soil;

                SoilManager.urbanFarmData.Item2.ForEach((CropSaveState crop) =>
                {
                    Debug.LogWarning(crop.seedToGrow + "\n Health: " + crop.health + "\n Growth: " + crop.growth + "\n State: " + crop.cropState.ToString());
                });
            }
        }
    }

    public void Sleep()
    {
        //Set the time 
        GameTimeStamp timestampOfNextDay = TimeManager.Instance.GetGameTimeStamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;

        TimeManager.Instance.SkipTime(timestampOfNextDay);

        PlayerStats.ResetMiniGameStatus();

        if (!debugging)
        {
            SaveManager.Save(ExportSaveState());

            StartCoroutine(SleepSequence());
        }
    }

    public IEnumerator FadeOut()
    {
        IsFading = true; // Prevent interaction
        fadeImage.gameObject.SetActive(true);
        yield return StartCoroutine(Fade(0, 1));
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(1, 0));
        fadeImage.gameObject.SetActive(false);
        IsFading = false; // Re-enable interaction
    }

    private IEnumerator SleepSequence()
    {
        CursorManager.Instance.UIOpened();

        yield return FadeOut();

        PlayerStats.RestoreStamina(100);

        yield return FadeIn();

        CursorManager.Instance.UIClosed();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }

    public GameSaveState ExportSaveState()
    {
        //Retrieving the Farm Data
        List<SoilSaveState> soilData = SoilManager.urbanFarmData.Item1;
        List<CropSaveState> cropData = SoilManager.urbanFarmData.Item2;

        //Retrieving the Inventory Data
        ItemSlotData[] storageSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Storage);
        ItemSlotData[] harvestlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest);

        ItemSlotData equippedStorageSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Storage);
        ItemSlotData equippedHarvestSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        //Time
        GameTimeStamp timestamp = TimeManager.Instance.GetGameTimeStamp();

        return new GameSaveState(soilData, cropData, storageSlots, harvestlots, equippedStorageSlot, equippedHarvestSlot, timestamp, PlayerStats.Money, PlayerStats.Stamina, PlayerStats.Hunger);
    }

    public void LoadSave()
    {
        GameSaveState save = SaveManager.Load();

        TimeManager.Instance.LoadTime(save.timestamp);

        ItemSlotData[] storageSlots = save.storageSlot;
        ItemSlotData equippedStorageSlot = save.equippedStorageSlot;

        ItemSlotData[] harvestSlots = save.harvestSlot;
        ItemSlotData equippedHarvestSlot = save.equippedHarvestSlot;

        NewInventoryManager.Instance.LoadInventory(storageSlots, equippedStorageSlot, harvestSlots, equippedHarvestSlot);

        SoilManager.urbanFarmData = new System.Tuple<List<SoilSaveState>, List<CropSaveState>>(save.soilData, save.cropData);

        PlayerStats.LoadStats(save.money, save.stamina, save.hunger);
    }
}
