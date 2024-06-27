using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopupTrigger : MonoBehaviour
{
    [Header("Trigger Size")]
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);

    [Header("UI Elements")]
    [Tooltip("Canvas of the GameObject")]
    public GameObject UIGuide;
    [Tooltip("World Space Text of the GameObject")]
    public GameObject nameHeader;

    [Header("Player Reference")]
    public Transform Player;

    private bool playerInRange = false;
    //public static bool isPlayerInTriggerZone = false;

    private static List<PopupTrigger> activeTriggers = new List<PopupTrigger>();

    private void Update()
    {
        CheckPlayerInRange();

        if (playerInRange)
        {
            if (!activeTriggers.Contains(this))
            {
                activeTriggers.Add(this);
                ActivateUI();
            }
        }
        else
        {
            if (activeTriggers.Contains(this))
            {
                activeTriggers.Remove(this);
                DeactivateUI();
                ListOfSoil.DeselectAll();
            }
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

    private void ActivateUI()
    {
        nameHeader.SetActive(true);
        UIGuide.SetActive(true);
    }

    private void DeactivateUI()
    {
        nameHeader.SetActive(false);
        UIGuide.SetActive(false);
    }

    public bool IsPlayerInRange()
    {
        return playerInRange;
    }

    public static List<PopupTrigger> GetActiveTriggers()
    {
        return activeTriggers;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a red box in the scene view to visualize the interaction area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
