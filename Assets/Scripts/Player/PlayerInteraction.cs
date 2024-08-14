using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMove playerMove;

    Soil selectedSoil = null;

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
            Soil soilIndicator = other.GetComponent<Soil>();
            SelectPot(soilIndicator);
            return;
        }

        if(selectedSoil != null)
        {
            selectedSoil.Select(false);
            selectedSoil = null;
        }
    }

    void SelectPot(Soil soilIndicator)
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
}
