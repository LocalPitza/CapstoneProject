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
    [SerializeField] private ItemSlotData[] seedSlots = new ItemSlotData[5];
    [SerializeField] private ItemSlotData equippedSeedSlot = null;

    [Header("Storage UI")]
    [SerializeField] private ItemSlotData[] storageSlots = new ItemSlotData[35];
    [SerializeField] private ItemSlotData equippedStorageSlot = null;
    public ItemSlotData equippedHarvestedSlot = null;
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
        ItemSlotData handToEquip = equippedStorageSlot;
        ItemSlotData[] inventoryToAlter = storageSlots;

        if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            handToEquip = equippedSeedSlot;
            inventoryToAlter = seedSlots;
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            handToEquip.AddQuantity(slotToAlter.quantity);

            slotToAlter.Empty();
        }
        else
        {
            //Not Stackable
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            EquipHandSlot(slotToEquip);

            //RenderHand();
        }

        NewUIManager.Instance.RenderInventory();

        /*if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            ItemData seedToSelect = seedSlots[slotIndex];

            seedSlots[slotIndex] = equippedSeedSlots;

            equippedSeedSlots = seedToSelect;
        }
        else //Storage Box
        {
            ItemData storageToSelect = storageSlots[slotIndex];

            storageSlots[slotIndex] = equippedStorageSlot;

            equippedStorageSlot = storageToSelect;

            //RenderHand();
        }

        NewUIManager.Instance.RenderInventory();*/
    }

    public void EquipToInventory(NewInventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = equippedStorageSlot;
        ItemSlotData[] inventoryToAlter = storageSlots;

        if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            handSlot = equippedSeedSlot;
            inventoryToAlter = seedSlots;
        }

        //Try stacking the hand slot
        if(!StackItemToInventory(handSlot, inventoryToAlter))
        {
            for(int i = 0;  i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);

                    handSlot.Empty();

                    break;
                }
            }
        }

        #region Old Code for starting index of the storage
        /*int startIndex = 15; // Start filling storage slots from index 15

        if (activeItemType == ActiveItemType.SelectedSeed && inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            for (int i = 0; i < seedSlots.Length; i++)
            {
                if (seedSlots[i] == null)
                {
                    seedSlots[i] = equippedSeedSlots;
                    equippedSeedSlots = null;
                    activeItemType = ActiveItemType.None;
                    break;
                }
            }
        }
        else // Storage
        {
            ItemData itemToPlace = (equippedHarvestedSlot != null) ? equippedHarvestedSlot : equippedStorageSlot;

            if (itemToPlace != null)
            {
                for (int i = startIndex; i < storageSlots.Length; i++)
                {
                    if (storageSlots[i] == null)
                    {
                        storageSlots[i] = itemToPlace;

                        // Clear the corresponding item reference
                        if (equippedHarvestedSlot != null)
                        {
                            equippedHarvestedSlot = null;
                        }
                        else if (equippedStorageSlot != null)
                        {
                            equippedStorageSlot = null;
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
        */
        #endregion

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

    //Iterate through each of the Items in the Invetory to see if it can be stacked
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                //Add to the Inventory slot's stack
                inventoryArray[i].AddQuantity(itemSlot.quantity);

                //Empty the item slot
                itemSlot.Empty();

                return true;
            }
        }
        //Can't find any slot that can be stacked
        return false;
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if(SlotEquipped(NewInventorySlot.InventoryType.Storage))
        {
            Instantiate(GetEquippedSlotStorage(NewInventorySlot.InventoryType.Storage).gameModel, handPoint);
        }
    }

    public bool IsStorage(ItemData item)
    {
        EquipmentData equipment = item as EquipmentData;
        if(equipment != null)
        {
            return true;
        }

        SeedData seed = item as SeedData;
        return seed != null;
    }

    #region Get and Checks

    public ItemData GetEquippedSlotStorage(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Storage)
        {
            return equippedStorageSlot.itemData;
        }
        return equippedSeedSlot.itemData;
    }

    public ItemSlotData GetEquippedSlot(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Storage)
        {
            return equippedStorageSlot;
        }
        return equippedSeedSlot;
    }

    public ItemSlotData[] GetInventorySlot(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Storage)
        {
            return storageSlots;
        }
        return seedSlots;
    }

    public bool SlotEquipped(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Storage)
        {
            return !equippedStorageSlot.IsEmpty();
        }
        return !equippedSeedSlot.IsEmpty();
    }
    #endregion


    public void EquipHandSlot(ItemData item)
    {
        if (IsStorage(item))
        {
            equippedStorageSlot = new ItemSlotData(item);
            //equippedHarvestedSlot = new ItemSlotData(item);
        }
        else
        {
            equippedSeedSlot = new ItemSlotData(item);
        }
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;

        if (IsStorage(item))
        {
            equippedStorageSlot = new ItemSlotData(itemSlot);
            //equippedHarvestedSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedSeedSlot = new ItemSlotData(itemSlot);
        }
    }


    #region Inventory Slot Validation
    private void OnValidate()
    {
        ValidateInventorySlot(equippedStorageSlot);
        ValidateInventorySlot(equippedHarvestedSlot);
        ValidateInventorySlot(equippedSeedSlot);

        ValidateInventorySlots(storageSlots);
        ValidateInventorySlots(seedSlots);
    }

    void ValidateInventorySlot(ItemSlotData slot)
    {
        if(slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach(ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    #endregion

}
