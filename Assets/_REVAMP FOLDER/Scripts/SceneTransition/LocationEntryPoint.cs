using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    [SerializeField] GameObject doorUI;
    [SerializeField] CinemachineVirtualCamera targetCamera;

    private bool playerInsideTrigger = false;

    private void Start()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (doorUI != null)
        {
            doorUI.SetActive(playerInsideTrigger);

            if (targetCamera != null)
            {
                Vector3 direction = doorUI.transform.position - targetCamera.transform.position;
                doorUI.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        ChangeScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered the trigger.");
            playerInsideTrigger = true;
            PlayerMove.isInTeleportTrigger = true; //Prevent interaction
            doorUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player exited the trigger.");
            playerInsideTrigger = false;
            PlayerMove.isInTeleportTrigger = false; //Allow interaction again
            doorUI.SetActive(false);
        }
    }

    void ChangeScene()
    {
        if (playerInsideTrigger && Input.GetKeyDown(InputManager.Instance.interactKey) && !GameStateManager.Instance.IsFading)
        {
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
            PlayerMove.isInTeleportTrigger = false;
        }
    }

}
