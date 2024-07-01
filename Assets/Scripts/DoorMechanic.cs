using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    //public Transform player;
    public Transform entryLocation;
    public GameObject playerObject;
    public GameObject doorUI;

    private bool playerInRange = false;

    /*private void Update()
    {
        if (PopupTrigger.GetActiveTriggers().Count > 0 && Input.GetKeyDown(KeyCode.F))
        {
            if (player != null && entryLocation != null && playerObject != null)
            {
                playerObject.SetActive(false);
                player.position = entryLocation.position;
                playerObject.SetActive(true);
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone");
            doorUI.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger zone");
            doorUI.SetActive(false);
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TeleportPlayer();
            doorUI.SetActive(false);
        }
    }

    private void TeleportPlayer()
    {
        if (playerObject != null && entryLocation != null)
        {
            playerObject.SetActive(false);
            playerObject.transform.position = entryLocation.position;
            playerObject.SetActive(true);
        }
    }
}
