using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMove playerMove;

    PottingSoil selectedSoil = null;

    void Start()
    {
        playerMove = transform.parent.GetComponent<PlayerMove>();
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 2)) 
        { 
            OnInteractableHit(hit);
        }
    }

    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        if(other.CompareTag("Pot"))
        {
            PottingSoil soilIndicator = other.GetComponent<PottingSoil>();
            SelectPot(soilIndicator);
            return;
        }

        if(selectedSoil != null)
        {
            selectedSoil.Select(false);
            selectedSoil = null;
        }
    }

    void SelectPot(PottingSoil soilIndicator)
    {
        if(selectedSoil != null)
        {
            selectedSoil.Select(false);
        }

        selectedSoil = soilIndicator;
        soilIndicator.Select(true);
    }

    public void Interact()
    {
        if(selectedSoil != null)
        {
            selectedSoil.Interact();
            return;
        }
        Debug.Log("No Soil");
    }

    public void HarvestableInteract()
    {
        Debug.Log("Selected");

        GameObject[] harvestableObjects = GameObject.FindGameObjectsWithTag("Harvestable");

        // Loop through all found objects
        foreach (GameObject harvestableObject in harvestableObjects)
        {
            Debug.Log("Harvesting");
            // Check if the object has a Harvestable component
            Harvestable harvest = harvestableObject.GetComponent<Harvestable>();
            if (harvest != null)
            {
                Debug.Log("Harv");
                harvest.Harvest();
                break; // Stop after the first harvestable object is harvested
            }
        }
    }
}
