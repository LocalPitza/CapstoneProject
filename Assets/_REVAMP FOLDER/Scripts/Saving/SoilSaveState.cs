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

            if (hoursElapsed > 23)
            {
                soilStatus = PottingSoil.SoilStatus.Digged;
            }
        }

        else if (soilStatus == PottingSoil.SoilStatus.Digged)
        {
            // Handle weeds based on elapsed time since last watered
            if (lastWatered != null)
            {
                int hoursSinceWatered = GameTimeStamp.CompareTimestamp(lastWatered, timestamp);

                // Change to Weeds if more than 24 hours since last watered
                if (hoursSinceWatered > 24)
                {
                    soilStatus = PottingSoil.SoilStatus.Weeds;
                }
            }
        }

    }
}
