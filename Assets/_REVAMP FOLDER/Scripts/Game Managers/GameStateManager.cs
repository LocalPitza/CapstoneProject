using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 1.0f;

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

    public void Sleep()
    {
        GameTimeStamp timestampOfNextDay = TimeManager.Instance.GetGameTimeStamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;
        Debug.Log(timestampOfNextDay.day + " " + timestampOfNextDay.hour + ":" + timestampOfNextDay.minute);

        TimeManager.Instance.SkipTime(timestampOfNextDay);

        SaveManager.Save(ExportSaveState());

        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        fadeImage.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(0, 1));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Fade(1, 0));

        fadeImage.gameObject.SetActive(false);
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
        //List<SoilSaveState> soilData = SoilManager.urbanFarmData.Item1;
        //List<CropSaveState> cropData = SoilManager.urbanFarmData.Item2;

        //Retrieving the Inventory Data
        ItemSlotData[] storageSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Storage);
        ItemSlotData[] harvestlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest);

        ItemSlotData equippedStorageSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Storage);
        ItemSlotData equippedHarvestSlot = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest);

        //Time
        GameTimeStamp timestamp = TimeManager.Instance.GetGameTimeStamp();

        return new GameSaveState(storageSlots, harvestlots, equippedStorageSlot, equippedHarvestSlot, timestamp);
    }

    public void LoadGame()
    {
        // Load the saved game state
        GameSaveState save = SaveManager.Load();

        if (save == null)
        {
            Debug.LogError("No save data found.");
            return;
        }

        SceneManager.LoadScene("RevampScene");

        // Restore the time from the save
        TimeManager.Instance.LoadTime(save.timestamp);

        // Restore the inventory
        ItemSlotData[] storageSlots = save.storageSlot;
        ItemSlotData equippedStorageSlot = save.equippedStorageSlot;

        ItemSlotData[] harvestSlots = save.harvestSlot;
        ItemSlotData equippedHarvestSlot = save.equippedHarvestSlot;

        NewInventoryManager.Instance.LoadInventory(storageSlots, equippedStorageSlot, harvestSlots, equippedHarvestSlot);

        Debug.Log("Game state loaded successfully!");
    }
}
