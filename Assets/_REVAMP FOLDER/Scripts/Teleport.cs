using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Set the teleport destination as a Transform.")]
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has a CharacterController (assumes this is the player)
        CharacterController characterController = other.GetComponent<CharacterController>();
        if (characterController != null)
        {
            if (teleportDestination != null)
            {
                // Start teleportation sequence with fade effect
                StartCoroutine(TeleportWithFade(characterController));
            }
            else
            {
                Debug.LogWarning("Teleport destination is not set.");
            }
        }
    }

    private IEnumerator TeleportWithFade(CharacterController characterController)
    {
        // Trigger fade-out
        yield return GameStateManager.Instance.FadeOut();

        // Disable CharacterController to avoid conflicts during teleportation
        characterController.enabled = false;

        // Teleport the player to the destination
        characterController.transform.position = teleportDestination.position;

        // Optionally reset player rotation
        characterController.transform.rotation = teleportDestination.rotation;

        // Re-enable the CharacterController
        characterController.enabled = true;

        // Trigger fade-in
        yield return GameStateManager.Instance.FadeIn();
    }
}
