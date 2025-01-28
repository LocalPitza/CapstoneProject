using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private bool playerInsideTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger.");
            playerInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited the trigger.");
            playerInsideTrigger = false;
        }
    }

    private void Update()
    {
        // Check for key press if the player is inside the trigger
        if (playerInsideTrigger && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            Debug.Log("Switching location.");
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }
    }
}
