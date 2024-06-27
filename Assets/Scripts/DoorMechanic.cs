using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanic : MonoBehaviour
{
    public Transform player;
    public Transform entryLocation;
    public GameObject playerObject;

    private void Update()
    {
        if (PopupTrigger.GetActiveTriggers().Count > 0 && Input.GetKeyDown(KeyCode.F))
        {
            playerObject.SetActive(false);
            player.position = entryLocation.position;
            playerObject.SetActive(true);
        }
    }
}
