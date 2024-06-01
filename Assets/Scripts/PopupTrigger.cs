using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public GameObject triggerObj;
    public GameObject pressKey;
    public Transform Player;

    public static bool isPlayerInTriggerZone = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerObj.SetActive(true);
            pressKey.SetActive(true);
            isPlayerInTriggerZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerObj.SetActive(false);
            pressKey.SetActive(false);
            isPlayerInTriggerZone = false;
        }
    }
}
