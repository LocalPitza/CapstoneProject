using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttendWork : MonoBehaviour
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
            MiniGameManager.instance.StartMiniGame();
        }
    }
}
