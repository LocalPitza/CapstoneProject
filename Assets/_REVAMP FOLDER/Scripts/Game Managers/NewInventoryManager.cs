using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventoryManager : MonoBehaviour
{
    public static NewInventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Seeds UI")]
    public ItemData[] seedsSlots = new ItemData[5];
    public ItemData selectedSeed = null;

    [Header("Storage UI")]
    public ItemData[] storageSlots = new ItemData[35];
    public ItemData selectedStorage = null;
    public ItemData harvestProduct = null;
    public Transform handPoint;

    public enum ActiveItemType
    {
        None,
        SelectedSeed,
        SelectedStorage,
        HarvestProduct
    }

    public ActiveItemType activeItemType = ActiveItemType.None;

    public void SetActiveItemType(ActiveItemType type)
    {
        activeItemType = type;
    }

    //Equipping
    public void InventoryToEquip(int slotIndex, NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            ItemData seedToSelect = seedsSlots[slotIndex];

            seedsSlots[slotIndex] = selectedSeed;

            selectedSeed = seedToSelect;
        }
        else //Storage Box
        {
            ItemData storageToSelect = storageSlots[slotIndex];

            storageSlots[slotIndex] = selectedStorage;

            selectedStorage = storageToSelect;

            //RenderHand();
        }

        NewUIManager.Instance.RenderInventory();
    }

    public void EquipToInventory(NewInventorySlot.InventoryType inventoryType)
    {
        int startIndex = 15; // Start filling storage slots from index 15

        if (activeItemType == ActiveItemType.SelectedSeed && inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            for (int i = 0; i < seedsSlots.Length; i++)
            {
                if (seedsSlots[i] == null)
                {
                    seedsSlots[i] = selectedSeed;
                    selectedSeed = null;
                    activeItemType = ActiveItemType.None;
                    break;
                }
            }
        }
        else // Storage
        {
            ItemData itemToPlace = (harvestProduct != null) ? harvestProduct : selectedStorage;

            if (itemToPlace != null)
            {
                for (int i = startIndex; i < storageSlots.Length; i++)
                {
                    if (storageSlots[i] == null)
                    {
                        storageSlots[i] = itemToPlace;

                        // Clear the corresponding item reference
                        if (harvestProduct != null)
                        {
                            harvestProduct = null;
                        }
                        else if (selectedStorage != null)
                        {
                            selectedStorage = null;
                        }
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("No item available to place in the storage.");
            }
        }

        #region Old Code
        /*if (inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            for(int i = 0; i < seedsSlots.Length; i++)
            {
                if (seedsSlots[i] == null)
                {
                    seedsSlots[i] = selectedSeed;

                    selectedSeed = null;
                    break;
                }
            }
        }
        else
        {
            if (harvestProduct != null)
            {
                for (int i = 0; i < storageSlots.Length; i++)
                {
                    if (storageSlots[i] == null)
                    {
                        storageSlots[i] = harvestProduct;
                        harvestProduct = null;
                        break;
                    }
                }
            }

            if (selectedStorage != null)
            {
                for (int i = 0; i < storageSlots.Length; i++)
                {
                    if (storageSlots[i] == null)
                    {
                        storageSlots[i] = selectedStorage;
                        selectedStorage = null;
                        break;
                    }
                }
            }

            //RenderHand();
        }*/
        #endregion

        NewUIManager.Instance.RenderInventory();
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if(selectedStorage != null)
        {
            Instantiate(selectedStorage.gameModel, handPoint);
        }
    }
}
