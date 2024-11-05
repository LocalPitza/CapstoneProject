using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BedInteract : MonoBehaviour
{
    private InteractMessage interactMessage;

    void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
    }

    void Update()
    {
        //For Debugging Purposes, GetKeyDown will be GetKey as of the moment
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKey(KeyCode.F))
        {
            //EnergyBar.Instance.SetFullEnergy();
            TimeManager.Instance.Tick();
        }
    }
}
