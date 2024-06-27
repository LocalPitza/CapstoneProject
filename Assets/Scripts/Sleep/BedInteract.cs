using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BedInteract : MonoBehaviour
{
    private DayCounter dayCounter;
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);
    public GameObject notification;
    private bool playerInRange = false;

    void Start()
    {
        dayCounter = FindObjectOfType<DayCounter>();
        notification.SetActive(false);
    }

    void Update()
    {
        CheckPlayerInRange();

        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            DayCounter.Instance.AdvanceDay();
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
                notification.SetActive(true);
                playerInRange = true;
                break;
            }
            else
            {
                notification.SetActive(false);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a red box in the scene view to visualize the interaction area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
