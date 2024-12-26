using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    public static SoilManager Instance { get; private set; }

    public static Tuple<List<SoilSaveState>, List<CropSaveState>> urbanFarmData = null;

    public GameObject[] plantingAreas;

    List<PottingSoil> soilPlots = new List<PottingSoil>();

    List<SoilSaveState> soilData = new List<SoilSaveState>();
    List<CropSaveState> cropData = new List<CropSaveState>();

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

    private void OnEnable()
    {
        RegisterSoilPlots();
        StartCoroutine(LoadFarmData());
    }

    IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();
        if (urbanFarmData != null)
        {
            //Load any save data
            ImportSoilData(urbanFarmData.Item1);
            ImportCropData(urbanFarmData.Item2);
        }
    }

    private void OnDestroy()
    {
        urbanFarmData = new Tuple<List<SoilSaveState>, List<CropSaveState>>(soilData, cropData);
        cropData.ForEach((CropSaveState crop) => {
            Debug.Log(crop.seedToGrow);
        });
    }

    #region Register and Deregistering
    void RegisterSoilPlots()
    {
        /*foreach (Transform soilTransform in transform)
        {
            PottingSoil potSoil = soilTransform.GetComponent<PottingSoil>();
            soilPlots.Add(potSoil);

            soilData.Add(new SoilSaveState());
            potSoil.id = soilPlots.Count - 1;
        }*/

        foreach (GameObject plantingArea in plantingAreas)
        {
            foreach (Transform soilTransform in plantingArea.transform)
            {
                PottingSoil potSoil = soilTransform.GetComponent<PottingSoil>();
                if (potSoil != null)
                {
                    soilPlots.Add(potSoil);
                    soilData.Add(new SoilSaveState());
                    potSoil.id = soilPlots.Count - 1; // Assign unique ID
                }
            }
        }

        Debug.Log($"Registered {soilPlots.Count} soil plots.");
    }
    
    //Registers the crop onto the Instance
    public void RegisterCrop(int landID, SeedData seedToGrow, NewCropBehaviour.CropState cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));
    }

    public void DeregisterCrop(int soilID)
    {
        cropData.RemoveAll(x => x.soilID == soilID);
    }
    #endregion

    #region State Changes
    //Update the corresponding Soil Data on ever change to the soil's state
    public void OnSoilStateChange(int id, PottingSoil.SoilStatus soilStatus, GameTimeStamp lastWatered)
    {
        soilData[id] = new SoilSaveState(soilStatus, lastWatered);
    }

    public void OnCropStateChange(int soilID, NewCropBehaviour.CropState cropState, int growth, int health)
    {
        int cropIndex = cropData.FindIndex(x => x.soilID == soilID);

        if (cropIndex == -1)
        {
            Debug.LogError($"No crop found with landID {soilID}");
            return;
        }

        string seedToGrow = cropData[cropIndex].seedToGrow;
        cropData[cropIndex] = new CropSaveState(soilID, seedToGrow, cropState, growth, health);
    }
    #endregion

    #region LoadingData
    public void ImportSoilData(List<SoilSaveState> soilDatasetToLoad)
    {
        for(int i = 0; i < soilDatasetToLoad.Count; i++)
        {
            SoilSaveState soilDataToLoad = soilDatasetToLoad[i];
            soilPlots[i].LoadSoilData(soilDataToLoad.soilStatus, soilDataToLoad.lastWatered);
        }

        soilData = soilDatasetToLoad;
    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            PottingSoil soilToPlant = soilPlots[cropSave.soilID];
            NewCropBehaviour cropToPlant = soilToPlant.SpawnCrop();

            if (cropToPlant == null)
            {
                Debug.LogError($"Failed to spawn crop for soil ID {cropSave.soilID}");
                continue;
            }

            if (NewInventoryManager.Instance == null)
            {
                Debug.LogError("NewInventoryManager.Instance is null.");
                continue;
            }

            if (NewInventoryManager.Instance.itemIndex == null)
            {
                Debug.LogError("ItemIndex is null.");
                continue;
            }

            Debug.Log($"Looking for seed: {cropSave.seedToGrow}");
            foreach (var item in NewInventoryManager.Instance.itemIndex.items)
            {
                Debug.Log($"Item in index: {item.name}");
            }

            SeedData seedToGrow = (SeedData)NewInventoryManager.Instance.itemIndex.GetItemFromString(cropSave.seedToGrow);

            if (seedToGrow == null)
            {
                Debug.LogWarning($"Seed '{cropSave.seedToGrow}' not found in ItemIndex. Skipping crop planting for soil ID {cropSave.soilID}.");
                continue;
            }

            cropToPlant.LoadCrop(cropSave.soilID, seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);
            Debug.Log($"Loaded crop with seed {seedToGrow.name} on soil ID {cropSave.soilID}");
        }

        /*cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            PottingSoil soilToPlant = soilPlots[cropSave.soilID];

            NewCropBehaviour cropToPlant = soilToPlant.SpawnCrop();
            Debug.Log(cropToPlant.gameObject);

            SeedData seedToGrow = (SeedData)NewInventoryManager.Instance.itemIndex.GetItemFromString(cropSave.seedToGrow);

            cropToPlant.LoadCrop(cropSave.soilID, seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);
        }*/
    }
    #endregion
}
