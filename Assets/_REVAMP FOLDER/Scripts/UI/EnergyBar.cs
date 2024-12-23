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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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

    public void DeductEnergy(int amount)
    {
        if (currentEnergy > 0)
        {
            currentEnergy -= amount; // Deduct energy by the given amount
            barSlider.value = currentEnergy; // Update the Slider value
            Debug.Log("Energy deducted: " + amount + ", Current energy: " + currentEnergy);
        }
    }

    public void SetFullEnergy()
    {
        currentEnergy = (int)barSlider.maxValue;
        barSlider.value = currentEnergy;
        Debug.Log("Energy set to full: " + currentEnergy);
    }
}
