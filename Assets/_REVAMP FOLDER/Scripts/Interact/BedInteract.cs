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
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKey(KeyCode.F))
        {
            NewUIManager.Instance.TriggerYesNoPrompt("Do you want to Sleep?", GameStateManager.Instance.Sleep);
        }
    }
}
