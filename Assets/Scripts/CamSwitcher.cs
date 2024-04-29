using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class CamSwitcher : MonoBehaviour
{

    public Transform Player;
    public CinemachineVirtualCamera activeCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCam.Priority = 1;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCam.Priority = 0;
        }
    }
}
