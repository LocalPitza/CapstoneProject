using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMove playerMove;

    //[HideInInspector]
    public PottingSoil selectedSoil = null;
    InteractableObject selectedInteractableObject = null;

    ShowUISeeds showUISeeds;
    //[HideInInspector]
    public bool harvestableHit = false;

    [Header("Messages to Player")]
    [Header("Harvesting Vegestable")]
    [SerializeField] private string equipGlove;

    [Header("Harvesting Fruits")]

    public static TextMeshProUGUI message;

    void Start()
    {
        playerMove = transform.parent.GetComponent<PlayerMove>();
    }

    void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down * 2, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2)) 
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
        ItemData playerToolSlot = NewInventoryManager.Instance.GetEquippedSlotStorage(NewInventorySlot.InventoryType.Storage);
        EquipmentData equipmentTool = playerToolSlot as EquipmentData;

        #region Message to Player if correct equipment
        //If Plalyer is not using the right tool for Harvesting
        if (equipmentTool == null || equipmentTool.toolType != EquipmentData.ToolType.HandGloves)
        {
            message.text = equipGlove;
            StartCoroutine(ClearMessageAfterDelay(2f));
            return;
        }
        else
        {
            message.text = "";
        }
        #endregion

        if (NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Storage))
        {
            NewInventoryManager.Instance.EquipToInventory(NewInventorySlot.InventoryType.Storage);
            return;
        }

        if(selectedInteractableObject != null)
        {
            selectedInteractableObject.PickUp();
        }
    }

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        message.text = "";
    }
}
