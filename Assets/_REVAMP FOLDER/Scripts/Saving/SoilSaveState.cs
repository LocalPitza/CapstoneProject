using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoilSaveState
{
    public PottingSoil.SoilStatus soilStatus;
    public GameTimeStamp lastWatered;

    public SoilSaveState(PottingSoil.SoilStatus soilStatus, GameTimeStamp lastWatered)
    {
        this.soilStatus = soilStatus;
        this.lastWatered = lastWatered;
    }
}
