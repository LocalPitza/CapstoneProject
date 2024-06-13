using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class CamSwitcher : MonoBehaviour
{

    //there is a delay whenever the objects are being disabled


    public Transform Player;
    public CinemachineVirtualCamera activeCam;
    public List<GameObject> listOfObjects = new List<GameObject>();
    private int currentActiveIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCam.Priority = 1;
            for(int i = 0; i < listOfObjects.Count; i++)
            {
                listOfObjects[i].SetActive(true);
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeCam.Priority = 0;
            for (int i = 0; i < listOfObjects.Count; i++)
            {
                listOfObjects[i].SetActive(false);
            }
        }
    }
}
