using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PottingSoil : MonoBehaviour, ITimeTracker
{
    public enum SoilStatus
    {
        Soil, Digged, Watered
    }

    public SoilStatus soilStatus;
    private SoilStatus previousStatus;

    public Material soilMat, diggedMat, wateredMat;
    new Renderer renderer;

    public GameObject select;

    GameTimeStamp timeWatered;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default Material
        SwitchSoilStatus(SoilStatus.Soil);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchSoilStatus(SoilStatus statusToSwitch)
    {
        if (statusToSwitch == SoilStatus.Watered)
        {
            previousStatus = soilStatus;
            timeWatered = TimeManager.Instance.GetGameTimeStamp();
        }

        soilStatus = statusToSwitch;

        Material materialToSwitch = soilMat;
        switch (statusToSwitch)
        {
            case SoilStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case SoilStatus.Digged:
                materialToSwitch = diggedMat;
                break;
            case SoilStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimeStamp();
                break;
        }
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {
        ItemData playerToolSlot = NewInventoryManager.Instance.selectedTool;
        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    if (soilStatus != SoilStatus.Digged)
                    {
                        SwitchSoilStatus(SoilStatus.Digged);
                    }
                    break;

                case EquipmentData.ToolType.WateringCan:
                    if (soilStatus != SoilStatus.Watered)
                    {
                        SwitchSoilStatus(SoilStatus.Watered);
                    }
                    break;
            }
        }
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        if(soilStatus == SoilStatus.Watered)
        {
            int hoursElapsed = GameTimeStamp.CompareTimestamp(timeWatered, timestamp);
            Debug.Log(hoursElapsed + "since watered");

            if(hoursElapsed > 24)
            {
                SwitchSoilStatus(previousStatus);
            }
        }
    }
}
