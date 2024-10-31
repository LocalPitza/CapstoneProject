using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMove playerMove;

    [HideInInspector]
    public PottingSoil selectedSoil = null;
    InteractableObject selectedInteractableObject = null;

    ShowUISeeds showUISeeds;
    public bool harvestableHit = false;

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

            showUISeeds = other.GetComponent<ShowUISeeds>();

            return;
        }

        if (other.CompareTag("Harvestable"))
        {
            Debug.Log("Turnip Found");
            selectedInteractableObject = other.GetComponent<InteractableObject>();
            harvestableHit = true;
            return;
        }

        if(selectedInteractableObject != null)
        {
            selectedInteractableObject = null;
        }

        if(selectedSoil != null)
        {
            selectedSoil.Select(false);
            selectedSoil = null;
        }

        showUISeeds = null;
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

            if (showUISeeds != null && selectedSoil.soilStatus == PottingSoil.SoilStatus.Digged)
            {
                showUISeeds.ToggleUI();
                NewUIManager.Instance.RenderInventory();
            }

            return;
        }
    }

    public void HarvestInteract()
    {
        ItemData playerToolSlot = NewInventoryManager.Instance.selectedTool;
        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        if (equipmentTool == null || equipmentTool.toolType != EquipmentData.ToolType.HandGloves)
        {
            Debug.Log("You need to equip Hand Gloves to harvest.");
            return;
        }

        if (NewInventoryManager.Instance.selectedPocket != null)
        {
            NewInventoryManager.Instance.EquipToInventory(NewInventorySlot.InventoryType.PlayerPocket);
            return;
        }

        if(selectedInteractableObject != null)
        {
            selectedInteractableObject.PickUp();
        }
    }
}
