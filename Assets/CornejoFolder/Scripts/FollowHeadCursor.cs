using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHeadCursor : MonoBehaviour
{
public Transform playerHead; // Assign the player’s head Transform
    public Transform playerBody; // Assign the player's body Transform
    public Camera mainCamera; // The main game camera
    public float rotationSpeed = 5f; // Speed of head rotation

    [Header("Horzontal Constraints (Left/Right)")]
    public float maxYawAngle = 60f;

    [Header("Vertical Constraints (Up/Down)")]
    public float maxPitchAngle = 20f;
    public float minPitchAngle = -10f;

    private float currentYaw = 0f;
    private float currentPitch = 0f;

    void Update()
    {
        if (playerHead == null || playerBody == null || mainCamera == null)
            return;

        Vector3 mouseScreenPos = Input.mousePosition;

        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane + 2f));

        Vector3 lookDirection = worldMousePos - playerHead.position;

        lookDirection.Normalize();

        Vector3 localLookDir = playerBody.InverseTransformDirection(lookDirection);

        float targetYaw = Mathf.Atan2(localLookDir.x, localLookDir.z) * Mathf.Rad2Deg;

        currentYaw = Mathf.Clamp(targetYaw, -maxYawAngle, maxYawAngle);

        float targetPitch = Mathf.Asin(localLookDir.y) * Mathf.Rad2Deg;

        currentPitch = Mathf.Clamp(targetPitch, minPitchAngle, maxPitchAngle);

        Quaternion finalRotation = Quaternion.Euler(currentPitch, playerBody.eulerAngles.y + currentYaw, 0);

        playerHead.rotation = Quaternion.Slerp(playerHead.rotation, finalRotation, Time.deltaTime * rotationSpeed);
        
    }
}
