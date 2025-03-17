using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnabled : MonoBehaviour
{
    public List<GameObject> listOfObjects = new List<GameObject>();
    private int currentActiveIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            for (int i = 0; i < listOfObjects.Count; i++)
            {
                listOfObjects[i].SetActive(false);
            }
        }
    }
}
