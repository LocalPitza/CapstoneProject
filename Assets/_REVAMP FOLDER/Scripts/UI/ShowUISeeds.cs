using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUISeeds : MonoBehaviour
{
    [SerializeField] GameObject plantingSeedsUI;

    private PottingSoil pottingSoil;
    private bool hasDug = false;

    void Start()
    {
        plantingSeedsUI.SetActive(false);
        pottingSoil = GetComponent<PottingSoil>();
    }

    public void ToggleUI()
    {
        if (pottingSoil != null && pottingSoil.soilStatus == PottingSoil.SoilStatus.Digged)
        {
            if (!hasDug)
            {
                hasDug = true;
            }
            else
            {
                plantingSeedsUI.SetActive(!plantingSeedsUI.activeSelf);
            }
        }
    }

    public bool IsUIActive()
    {
        return plantingSeedsUI.activeSelf;
    }
}
