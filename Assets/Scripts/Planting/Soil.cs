using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public enum SoilStatus
    {
        Dry, Digged, Watered
    }

    public SoilStatus soilStatus;
    public Material soilMat, wateredMat;
    public GameObject digged;
    new Renderer renderer;

    private int lastWateredDay;
    public int daysUntilDry;

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default Status
        SwitchLandStatus(SoilStatus.Dry);

        DayCounter.Instance.OnDayAdvanced += CheckIfSoilShouldDry; // Subscribe to the day advanced event
    }

    private void OnDestroy()
    {
        DayCounter.Instance.OnDayAdvanced -= CheckIfSoilShouldDry; // Unsubscribe from the event
    }

    //Change Materials on Soil
    public void SwitchLandStatus(SoilStatus statusSwicth)
    {
        soilStatus = statusSwicth;
        Material materialSwitch = soilMat;

        switch (statusSwicth)
        {
            case SoilStatus.Digged:
                digged.SetActive(true);
                break;

            case SoilStatus.Dry:
                materialSwitch = soilMat;
                break;

            case SoilStatus.Watered:
                materialSwitch = wateredMat;
                lastWateredDay = DayCounter.Instance.GetCurrentDay();
                break;
        }

        renderer.material = materialSwitch;
    }

    public void Interact()
    {
        ItemData toolSlot = InventoryManager.Instance.equipedTool;

        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if(equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.HandTrowel:
                    SwitchLandStatus(SoilStatus.Digged);
                    break;

                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(SoilStatus.Watered);
                    break;
            }
        }
    }

    private void CheckIfSoilShouldDry()
    {
        if (soilStatus == SoilStatus.Watered)
        {
            int currentDay = DayCounter.Instance.GetCurrentDay();
            if (currentDay - lastWateredDay >= daysUntilDry)
            {
                SwitchLandStatus(SoilStatus.Dry);
            }
        }
    }
}
