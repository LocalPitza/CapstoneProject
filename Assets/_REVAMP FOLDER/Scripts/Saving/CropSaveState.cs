using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CropSaveState
{
    public int soilID;

    public string seedToGrow;
    public NewCropBehaviour.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int soilID, string seedToGrow, NewCropBehaviour.CropState cropState, int growth, int health)
    {
        this.soilID = soilID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }
}
