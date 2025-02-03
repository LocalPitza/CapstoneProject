using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractMessage : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);
    private bool playerInRange = false;

    public GameObject guideUI;

    private void Start()
    {
        if(guideUI != null)
        {
            guideUI.SetActive(false);
        }
    }

    private void Update()
    {
        CheckPlayerInRange();

        if (guideUI != null)
        {
            guideUI.SetActive(playerInRange);
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
                playerInRange = true;
                break;
            }
        }
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }


}
