using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : BarsUnit
{
    public static EnergyBar Instance { get; private set; }

    private int currentEnergy;

    [Header("Planting Cost")]
    public int ammountPlantEnergy;

    [Header("Working Cost")]
    public int ammountWorkEnergy;

    void Start()
    {
        currentEnergy = energy;
        barSlider.maxValue = energy;
        barSlider.value = currentEnergy;
    }

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        UpdateEnergyBar();
    }

    void UpdateEnergyBar()
    {
        barSlider.value = currentEnergy;
    }

    public void DeductEnergy(int ammount)
    {
        Debug.Log("Current Energy" + ammount);
    }
}
