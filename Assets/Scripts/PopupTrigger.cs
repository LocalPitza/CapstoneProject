using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PopupTrigger : MonoBehaviour
{
    [Header("Trigger Size")]
    [SerializeField] Vector3 boxSize = new Vector3(2f, 2f, 2f);

    [Header("UI Elements")]
    [Tooltip("Canvas of the GameObject")]
    [SerializeField] GameObject UIGuide;
    [Tooltip("World Space Text of the GameObject")]
    [SerializeField] GameObject nameHeader;

    private bool playerInRange = false;

    private static List<PopupTrigger> activeTriggers = new List<PopupTrigger>();

    private void Start()
    {
        if (!nameHeader)
        {
            nameHeader = null; //Can be Null
        }
    }

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
            else
            {
                playerInRange = false;
            }
        }
    }

    private void ActivateUI()
    {
        if (nameHeader != null)
        {
            nameHeader.SetActive(true);
        }
        UIGuide.SetActive(true);
    }

    private void DeactivateUI()
    {
        if (nameHeader != null)
        {
            nameHeader.SetActive(false);
        }
        UIGuide.SetActive(false);
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
