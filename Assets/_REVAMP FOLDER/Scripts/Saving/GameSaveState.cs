using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    //Farming Data
    public List<SoilSaveState> soilData;
    public List<CropSaveState> cropData;

    //Inventory
    public ItemSlotData[] storageSlot;
    public ItemSlotData[] harvestSlot;

    public ItemSlotData equippedStorageSlot;
    public ItemSlotData equippedHarvestSlot;

    //Time
    public GameTimeStamp timestamp;

    //PlayerStats
    public int money;
    public int stamina;

    public GameSaveState(List<SoilSaveState> soilData, List<CropSaveState> cropData, ItemSlotData[] storageSlot, ItemSlotData[] harvestSlot, ItemSlotData equippedStorageSlot, ItemSlotData equippedHarvestSlot, GameTimeStamp timestamp, int money, int stamina)
    {
        this.soilData = soilData;
        this.cropData = cropData;
        this.storageSlot = storageSlot;
        this.harvestSlot = harvestSlot;
        this.equippedStorageSlot = equippedStorageSlot;
        this.equippedHarvestSlot = equippedHarvestSlot;
        this.timestamp = timestamp;
        this.money = money;
        this.stamina = stamina;
    }
}
