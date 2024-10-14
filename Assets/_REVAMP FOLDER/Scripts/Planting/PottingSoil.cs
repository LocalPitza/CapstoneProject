using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PottingSoil : MonoBehaviour
{
    public enum SoilStatus
    {
        Soil, Digged, Watered
    }

    public SoilStatus soilStatus;

    public Material soilMat, diggedMat, wateredMat;
    new Renderer renderer;

    public GameObject select;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        //Default Material
        SwitchSoilStatus(SoilStatus.Soil);
    }

    public void SwitchSoilStatus(SoilStatus statusToSwitch)
    {
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
        Debug.Log("Interacting");
        SwitchSoilStatus(SoilStatus.Digged);
    }
}
