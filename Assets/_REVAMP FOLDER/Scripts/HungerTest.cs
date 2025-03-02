using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerTest : MonoBehaviour
{
    [SerializeField] private float hungerDecreaseInterval = 1f;
    [SerializeField] private int hungerDecreaseAmount = 1;
    [SerializeField] private GameObject medicalPrompt; // Reference to the MedicalPrompt UI

    private float hungerTimer;

    private void Update()
    {
        // Stop hunger decrease if medicalPrompt is active
        if (medicalPrompt != null && medicalPrompt.activeSelf)
        {
            return; // Skip hunger reduction
        }

        hungerTimer += Time.deltaTime;
        if (hungerTimer >= hungerDecreaseInterval)
        {
            PlayerStats.HungerStat(-hungerDecreaseAmount);
            hungerTimer = 0;
        }
    }
}
