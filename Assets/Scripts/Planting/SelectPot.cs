using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPot : MonoBehaviour
{
    public GameObject plantInventory;

    PlantGrowth plantGrowth;

    private void Start()
    {
        plantGrowth = GameObject.FindObjectOfType<PlantGrowth>();
    }

    void Update()
    {
        if (PopupTrigger.isPlayerInTriggerZone && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Create a layer mask that includes only the "Pot" layer
            int layerMask = LayerMask.GetMask("Pot");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Hit: " + hit.collider.name);
                plantInventory.SetActive(true);
                plantGrowth.StartGrowth();
            }
            else
            {
                Debug.Log("No Hit");
            }
        }
    }
}
