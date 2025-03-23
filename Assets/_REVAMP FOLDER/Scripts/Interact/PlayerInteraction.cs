using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMove playerMove;

    PottingSoil selectedSoil = null;
    InteractableObject selectedInteractableObject = null;

    [Header("Messages to Player")]
    [Header("Harvesting Vegestable")]
    [SerializeField] string equipGloveMessage;

    [Header("Unequip Harvested Item")]
    [SerializeField] string unequipMessage;

    [Header("Harvesting Fruits")]
    public TextMeshProUGUI message; 
    EquipmentData equipmentTool;

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
        else
        {
            // Ensure deselection happens if nothing is hit
            if (selectedSoil != null)
            {
                selectedSoil.Select(false);
                selectedSoil = null;
            }
            if (selectedInteractableObject != null)
            {
                selectedInteractableObject = null;
            }
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

        if (other.CompareTag("Harvestable"))
        {
            selectedInteractableObject = other.GetComponent<InteractableObject>();
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
        ItemData toolSlot = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Storage);
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        // Check if the player has an equipped item in the Harvest slot
        if (NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Harvest))
        {
            ItemData handSlotItem = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Harvest);

            if (handSlotItem == null)
            {
                return;
            }

            if (handSlotItem is FoodData)
            {
                FoodData foodData = handSlotItem as FoodData;
                foodData.OnConsume();
                FindObjectOfType<SoundManager>().Play("ConsumeSFX");
                //Consume it
                NewInventoryManager.Instance.ConsumeItem(NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest));
            }
            else
            {
                Debug.Log("Hand is full with Harvested Crop");
                message.text = unequipMessage;
                StartCoroutine(ClearMessageAfterDelay(2f));
                return;
            }
        }

        if (selectedSoil != null)
        {
            if (toolSlot is SeedData)
            {
                // If the player is holding a seed, plant it
                PlayerStats.UseStamina(5);
                FindObjectOfType<SoundManager>().Play("HarvestSFX");
                selectedSoil.Interact();
                return;
            }

            if (equipmentTool != null)
            {
                EquipmentData.ToolType toolType = equipmentTool.toolType;

                switch (toolType)
                {
                    case EquipmentData.ToolType.HandTrowel:
                        PlayerStats.UseStamina(5);
                        FindObjectOfType<SoundManager>().Play("DigSFX");
                        selectedSoil.Interact();
                        break;

                    case EquipmentData.ToolType.WateringCan:
                        PlayerStats.UseStamina(5);
                        FindObjectOfType<SoundManager>().Play("WaterSFX");
                        selectedSoil.Interact();
                        break;

                    case EquipmentData.ToolType.Hoe:
                        PlayerStats.UseStamina(5);
                        FindObjectOfType<SoundManager>().Play("RemoveSFX");
                        selectedSoil.Interact();
                        break;

                    default:
                        Debug.LogWarning("This tool can't be used here.");
                        message.text = "This tool can't be used here.";
                        StartCoroutine(ClearMessageAfterDelay(2f));
                        break;
                }
                return;
            }

            Debug.LogWarning("No valid item equipped for interaction.");
            message.text = " ";
            StartCoroutine(ClearMessageAfterDelay(2f));
        }
    }

    public void HarvestInteract()
    {
        //If the Player is holding a Harvested Fruit/Vegetable, keep it first before harvesting again
        if (NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Harvest))
        {
            NewInventoryManager.Instance.EquipToInventory(NewInventorySlot.InventoryType.Harvest);
            FindObjectOfType<SoundManager>().Play("HarvestSFX");
            return;
        }

        // Check if gloves are equipped in the tool slot
        ItemData toolSlot = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Storage);
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool == null || equipmentTool.toolType != EquipmentData.ToolType.HandGloves)
        {
            // Display message if gloves are not equipped
            message.text = equipGloveMessage;
            StartCoroutine(ClearMessageAfterDelay(2f));
            return;
        }

        if (selectedInteractableObject != null)
        {
            selectedInteractableObject.PickUp();
        }
    }

    /*public void HarvestKeep()
    {
        if (NewInventoryManager.Instance.SlotEquipped(NewInventorySlot.InventoryType.Harvest))
        {
            NewInventoryManager.Instance.EquipToInventory(NewInventorySlot.InventoryType.Harvest);
            return;
        }
    }*/

    private IEnumerator ClearMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        message.text = "";
    }

    public bool HasHarvestable()
    {
        return selectedInteractableObject != null;
    }
}
