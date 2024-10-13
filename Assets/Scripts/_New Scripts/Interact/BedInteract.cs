using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BedInteract : MonoBehaviour
{
    private EnergyBar energyBar;
    private InteractMessage interactMessage;

    void Start()
    {
        energyBar = EnergyBar.Instance;
        interactMessage = GetComponent<InteractMessage>();
    }

    void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Advance Day");
            energyBar.SetFullEnergy();
        }
    }
}
