using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDoor : MonoBehaviour
{
    public Transform player;
    public Transform targetPosition;
    public GameObject playerObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject.SetActive(false);
            player.position = targetPosition.position;
            playerObject.SetActive(true);
        }
    }
}
