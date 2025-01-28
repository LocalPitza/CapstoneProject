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
        //Find its index in the list from the landID and remove it
        cropData.RemoveAll(x => x.soilID == soilID);
    }
    #endregion

    #region State Changes
    //Update the corresponding Soil Data on ever change to the soil's state
    public void OnSoilStateChange(int id, PottingSoil.SoilStatus soilStatus, GameTimeStamp lastWatered)
    {
        soilData[id] = new SoilSaveState(soilStatus, lastWatered);
    }

    //Update the corresponding Crop Data on ever change to the Land's state
    public void OnCropStateChange(int soilID, NewCropBehaviour.CropState cropState, int growth, int health)
    {
        //Find its index in the list from the landID
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
    //Load over the static farmData onto the Instance's landData
    public void ImportSoilData(List<SoilSaveState> soilDatasetToLoad)
    {
        for(int i = 0; i < soilDatasetToLoad.Count; i++)
        {
            //Get the individual land save state
            SoilSaveState soilDataToLoad = soilDatasetToLoad[i];
            //Load it up onto the Land instance
            soilPlots[i].LoadSoilData(soilDataToLoad.soilStatus, soilDataToLoad.lastWatered);
        }

        soilData = soilDatasetToLoad;
    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        //Load over the static farmData onto the Instance's cropData
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            PottingSoil soilToPlant = soilPlots[cropSave.soilID];

            NewCropBehaviour cropToPlant = soilToPlant.SpawnCrop();

            SeedData seedToGrow = (SeedData)NewInventoryManager.Instance.itemIndex.GetItemFromString(cropSave.seedToGrow);

            cropToPlant.LoadCrop(cropSave.soilID, seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);

            Debug.Log($"Loaded crop with seed {seedToGrow.name} on soil ID {cropSave.soilID}");
        }
    }
    #endregion
}
