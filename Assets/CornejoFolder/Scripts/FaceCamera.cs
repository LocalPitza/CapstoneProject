using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;
    private float originalXRotation;

    private void Start()
    {
        // Cache the main camera transform
        mainCamera = Camera.main.transform;
        
        // Store the original X rotation
        originalXRotation = transform.rotation.eulerAngles.x;
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
            return;
        }

        // Get the direction from this object to the camera
        Vector3 directionToCamera = mainCamera.position - transform.position;
        
        // Zero out the Y component to keep the object upright
        directionToCamera.y = 0;
        
        // Calculate the target rotation to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
        
        // Apply the rotation while preserving the original X rotation
        Vector3 euler = targetRotation.eulerAngles;
        euler.x = originalXRotation;
        transform.rotation = Quaternion.Euler(euler);
    }
}
