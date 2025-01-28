using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Set the teleport destination as a Transform.")]
    public Transform teleportDestination;

    private bool playerInsideTrigger = false; // Flag to track if the player is in the trigger

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the teleport trigger.");
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the flag when the player exits the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the teleport trigger.");
            playerInsideTrigger = false;
        }
    }

    private void Update()
    {
        // Check for the interaction key press if the player is inside the trigger
        if (playerInsideTrigger && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            if (teleportDestination != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the player object
                StartCoroutine(TeleportWithFade(player));
            }
            else
            {
                Debug.LogWarning("Teleport destination is not set.");
            }
        }
    }

    private IEnumerator TeleportWithFade(GameObject player)
    {
        // Trigger fade-out
        yield return GameStateManager.Instance.FadeOut();

        // Move the player to the teleport destination
        player.transform.position = teleportDestination.position;

        // Optionally reset player rotation
        player.transform.rotation = teleportDestination.rotation;

        // Trigger fade-in
        yield return GameStateManager.Instance.FadeIn();
    }
}
