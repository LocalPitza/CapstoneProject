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

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if (soilStatus == PottingSoil.SoilStatus.Watered)
        {
            int hoursElapsed = GameTimeStamp.CompareTimestamp(lastWatered, timestamp);
            Debug.LogWarning(hoursElapsed + " hours since this was watered");

            if (hoursElapsed > 24)
            {
                soilStatus = PottingSoil.SoilStatus.Digged;
            }
        }

    }
}
