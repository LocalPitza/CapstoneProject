using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractMessage : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(2f, 2f, 2f);
    private bool playerInRange = false;

    [Header("Message")]
    [SerializeField] private string interactionMessage;
    private TextMeshProUGUI message;

    private void Start()
    {
        MessageManager messageManager = FindObjectOfType<MessageManager>();
        if (messageManager != null)
        {
            message = messageManager.GetInteractionText();
            Debug.Log("MessageManager found and interactionText assigned.");
        }
        else
        {
            Debug.LogError("MessageManager not found in the scene.");
        }
    }

    private void Update()
    {
        CheckPlayerInRange();

        if (playerInRange)
        {
            if (message != null && message.text != interactionMessage)
            {
                message.text = interactionMessage;
            }
        }
        else if (message != null && message.text == interactionMessage)
        {
            message.text = " ";
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
