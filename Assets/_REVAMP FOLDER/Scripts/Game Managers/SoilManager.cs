using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    public static SoilManager Instance { get; private set; }
    private PottingSoil selectedSoil;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedSoil(PottingSoil soil)
    {
        selectedSoil = soil;
    }

    public void PlantSeed()
    {
        if (selectedSoil != null)
        {
            //selectedSoil.PlantSeed();
        }
    }
}
