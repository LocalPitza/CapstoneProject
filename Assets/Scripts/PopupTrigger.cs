using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopupTrigger : MonoBehaviour
{
    [Header("Trigger Size")]
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);

    [Header("UI Elements")]
    public GameObject nameOfLoc;
    public GameObject selectPot;

    [Header("Player Reference")]
    public Transform Player;

    private bool playerInRange = false;
    public static bool isPlayerInTriggerZone = false;

    private void Update()
    {
        CheckPlayerInRange();
        if (playerInRange)
        {
            isPlayerInTriggerZone = true;
        }
        else
        {
            ListOfSoil.DeselectAll();
            isPlayerInTriggerZone = false;
        }
    }

    private void CheckPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, boxSize / 2);
        playerInRange = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                nameOfLoc.SetActive(true);
                selectPot.SetActive(true);
                playerInRange = true;
                break;
            }
            else
            {
                nameOfLoc.SetActive(false);
                selectPot.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red box in the scene view to visualize the interaction area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
