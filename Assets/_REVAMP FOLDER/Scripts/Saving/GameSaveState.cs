using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    //Farming Data
    //public List<SoilSaveState> soilData;
    //public List<CropSaveState> cropData;

    //Inventory
    public ItemSlotData[] storageSlot;
    public ItemSlotData[] harvestSlot;

    public ItemSlotData equippedStorageSlot;
    public ItemSlotData equippedHarvestSlot;

    //Time
    public GameTimeStamp timestamp;

    public GameSaveState(ItemSlotData[] storageSlot, ItemSlotData[] harvestSlot, ItemSlotData equippedStorageSlot, ItemSlotData equippedHarvestSlot, GameTimeStamp timestamp)
    {
        //this.soilData = soilData;
        //this.cropData = cropData;
        this.storageSlot = storageSlot;
        this.harvestSlot = harvestSlot;
        this.equippedStorageSlot = equippedStorageSlot;
        this.equippedHarvestSlot = equippedHarvestSlot;
        this.timestamp = timestamp;
    }


}