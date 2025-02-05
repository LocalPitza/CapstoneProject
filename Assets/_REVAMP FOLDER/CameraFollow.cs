using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        // Find the virtual camera on this object (make sure the script is on the same object as the camera)
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            AssignPlayerToCamera();
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera not found on this GameObject.");
        }
    }

    void AssignPlayerToCamera()
    {
        // Find the player by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Assign the player's transform to the Look At and Follow properties
            virtualCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }
    }
}
