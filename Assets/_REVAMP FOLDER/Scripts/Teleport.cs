using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]

    // The destination Transform to teleport to
    public Transform teleportDestination;
    public GameObject guideUI;
    public CinemachineVirtualCamera targetCamera;

    // The tag to identify the player
    public string playerTag = "Player";

    private GameObject playerInTrigger;

    private void Start()
    {
        if(guideUI != null)
        {
            guideUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = other.gameObject;
            PlayerMove.isInTeleportTrigger = true; //Prevent interaction
            if (guideUI != null)
            {
                guideUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Clear the reference when the player leaves the trigger
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = null;
            PlayerMove.isInTeleportTrigger = false; //Allow interaction again
            if (guideUI != null)
            {
                guideUI.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (guideUI != null)
        {
            guideUI.SetActive(playerInTrigger);

            if (targetCamera != null)
            {
                Vector3 direction = guideUI.transform.position - targetCamera.transform.position;
                guideUI.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        // Check if the player is in the trigger and the F key is pressed
        if (playerInTrigger != null && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            StartCoroutine(TeleportWithFade());
        }
    }

    private IEnumerator TeleportWithFade()
    {
        PlayerMove.isUIOpen = true;

        // Trigger fade-out
        NewUIManager.Instance.FadeOutScreen();

        // Wait for 1 second (adjust if needed)
        yield return new WaitForSeconds(1f);

        // Teleport player
        playerInTrigger.transform.position = teleportDestination.position;
        playerInTrigger.transform.rotation = teleportDestination.rotation;
        Debug.Log("Player teleported to " + teleportDestination);

        // Trigger fade-in
        NewUIManager.Instance.FadeInScreen();

        // Wait for fade-in to complete
        yield return new WaitForSeconds(1f);

        // Reset fade effect
        NewUIManager.Instance.OnFadeInComplete();
        NewUIManager.Instance.ResetFadeDefaults();

        PlayerMove.isUIOpen = false;
    }
}
