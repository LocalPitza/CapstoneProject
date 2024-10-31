using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantSeed : MonoBehaviour
{
    public Button plantSeedButton;

    private void Start()
    {
        plantSeedButton.onClick.AddListener(OnPlantSeedButtonClick);
    }

    private void OnPlantSeedButtonClick()
    {
        SoilManager.Instance.PlantSeed();
    }
}
