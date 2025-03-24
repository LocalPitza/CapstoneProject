using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHeadLook : MonoBehaviour
{
    [Header("Settings")]
    public Transform headBone;        // The head bone to rotate
    public float detectionRange = 5f; // How close the player must be
    public float rotationSpeed = 5f;  // How fast the head turns
    public float maxHeadTurnAngle = 60f; // Limit to avoid unnatural rotation

    private Transform player;

    private void Start()
    {
        FindPlayer(); // Try to find the player on start
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Continuously check if the player exists (for scene changes)
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            LookAtPlayer();
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void LookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - headBone.position;
        directionToPlayer.y = 0; // Prevents unnatural head tilting

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        Quaternion limitedRotation = LimitRotation(targetRotation);

        headBone.rotation = Quaternion.Slerp(headBone.rotation, limitedRotation, Time.deltaTime * rotationSpeed);
    }

    Quaternion LimitRotation(Quaternion targetRotation)
    {
        Vector3 targetEuler = targetRotation.eulerAngles;
        targetEuler.y = Mathf.Clamp(targetEuler.y, -maxHeadTurnAngle, maxHeadTurnAngle);
        return Quaternion.Euler(targetEuler);
    }
}
