using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public enum SoilStatus
    {
        Dry, Watered
    }
    public SoilStatus soilStatus;

    public Material soilMat, wateredMat;
    new Renderer renderer;


    private void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default Status
        SwitchLandStatus(SoilStatus.Dry);
    }

    //Change Materials on Soil
    public void SwitchLandStatus(SoilStatus statusSwicth)
    {
        soilStatus = statusSwicth;
        Material materialSwitch = soilMat;

        switch (statusSwicth)
        {
            case SoilStatus.Dry:
                materialSwitch = soilMat;
                break;

            case SoilStatus.Watered:
                materialSwitch = wateredMat;
                break;
        }

        renderer.material = materialSwitch;
    }
}
