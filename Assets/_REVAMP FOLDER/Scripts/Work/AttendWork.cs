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
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(KeyCode.F))
        {
            // Check if the player has already played the mini-game
            if (!PlayerStats.HasPlayedMiniGame())
            {
                MiniGameManager.instance.StartMiniGame();
                // Mark the mini-game as played after starting it
                PlayerStats.MarkMiniGameAsPlayed();
            }
            else
            {
                Debug.Log("You have already played the mini-game.");
                // Optionally show a UI message that the mini-game has already been played
            }
        }
    }
}
