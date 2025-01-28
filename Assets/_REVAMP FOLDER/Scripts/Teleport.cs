using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]

    // The destination Transform to teleport to
    public Transform teleportDestination;

    // The tag to identify the player
    public string playerTag = "Player";

    private GameObject playerInTrigger;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Clear the reference when the player leaves the trigger
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = null;
        }
    }

    void Update()
    {
        // Check if the player is in the trigger and the F key is pressed
        if (playerInTrigger != null && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            // Teleport the player to the destination
            playerInTrigger.transform.position = teleportDestination.position;
            playerInTrigger.transform.rotation = teleportDestination.rotation;
        }
    }
}
