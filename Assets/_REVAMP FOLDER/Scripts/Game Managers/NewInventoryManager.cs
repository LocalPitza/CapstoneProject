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

    [Header("Player Tools")]
    [SerializeField] 
    private ItemSlotData[] playerToolsSlots = new ItemSlotData[5];
    [SerializeField] 
    private ItemSlotData selectedToolSlot = null;

    [Header("Player Pocket")]
    [SerializeField] 
    private ItemSlotData[] playerPocketSlots = new ItemSlotData[5];
    [SerializeField] 
    private ItemSlotData selectedPocketSlot = null;
    public Transform handPoint;

    [Header("Seeds UI")]
    [SerializeField] 
    private ItemSlotData[] seedsStorageSlots = new ItemSlotData[5];
    [SerializeField] 
    private ItemSlotData selectedSeedSlot = null;

    [Header("Storage UI")]
    [SerializeField] 
    private ItemSlotData[] inventoryStorageSlots = new ItemSlotData[15];
    [SerializeField] 
    private ItemSlotData selectedInventoryStorageSlot = null;

    //Equipping
    public void InventoryToEquip(int slotIndex, NewInventorySlot.InventoryType inventoryType)
    {
        /*
        if(inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            ItemData playerToolToEquip = playerToolsSlots[slotIndex];

            playerToolsSlots[slotIndex] = selectedToolSlot;

            selectedToolSlot = playerToolToEquip;

        }
        else if(inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            ItemData playerPocketToEquip = playerPocketSlots[slotIndex];

            playerPocketSlots[slotIndex] = selectedPocketSlot;

            selectedPocketSlot = playerPocketToEquip;

            RenderHand();
        }
        else if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            ItemData seedToSelect = seedsStorageSlots[slotIndex];

            seedsStorageSlots[slotIndex] = selectedSeedSlot;

            selectedSeedSlot = seedToSelect;
        }
        else //Storage Box
        {
            ItemData storageToSelect = inventoryStorageSlots[slotIndex];

            inventoryStorageSlots[slotIndex] = selectedStorageSlot;

            selectedStorageSlot = storageToSelect;
        }

        NewUIManager.Instance.RenderInventory();
        */
    }

    public void EquipToInventory(NewInventorySlot.InventoryType inventoryType)
    {
        /*
        if(inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            for(int i = 0; i < playerToolsSlots.Length; i++)
            {
                if(playerToolsSlots[i] == null)
                {
                    playerToolsSlots[i] = selectedToolSlot;

                    selectedToolSlot = null;
                    break;
                }
            }
        }
        else if (inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            for (int i = 0; i < playerPocketSlots.Length; i++)
            {
                if (playerPocketSlots[i] == null)
                {
                    playerPocketSlots[i] = selectedPocketSlot;

                    selectedPocketSlot = null;
                    break;
                }
            }

            RenderHand();
        }
        else if (inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            for(int i = 0; i < seedsStorageSlots.Length; i++)
            {
                if (seedsStorageSlots[i] == null)
                {
                    seedsStorageSlots[i] = selectedSeedSlot;

                    selectedSeedSlot = null;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventoryStorageSlots.Length; i++)
            {
                if (inventoryStorageSlots[i] == null)
                {
                    inventoryStorageSlots[i] = selectedStorageSlot;

                    selectedStorageSlot = null;
                    break;
                }
            }
        }

        NewUIManager.Instance.RenderInventory();
        */
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if(selectedPocketSlot != null)
        {
            Instantiate(GetEquippedSlotItem(NewInventorySlot.InventoryType.PlayerPocket).gameModel, handPoint);
        }
    }

    #region Gets and Checks
    //Get the Slot Item
    public ItemData GetEquippedSlotItem(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            return selectedToolSlot.itemData;
        } 
        else if(inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            return selectedPocketSlot.itemData;
        }
        else if(inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            return selectedInventoryStorageSlot.itemData;
        }
        return selectedInventoryStorageSlot.itemData;
    }

    //Get function for the Slots
    public ItemSlotData GetEquippedSlot(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            return selectedToolSlot;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            return selectedPocketSlot;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            return selectedSeedSlot;
        }
        return selectedInventoryStorageSlot;
    }

    public ItemSlotData[] GetInventorySlots(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            return playerToolsSlots;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            return playerPocketSlots;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            return seedsStorageSlots;
        }
        return inventoryStorageSlots;
    }

    public bool SlotEquipped(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            return selectedToolSlot != null;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            return selectedPocketSlot != null;
        }
        else if (inventoryType == NewInventorySlot.InventoryType.Seed)
        {
            return selectedSeedSlot != null;
        }
        return selectedInventoryStorageSlot != null;
    }

    public bool IsEquipment(ItemData item)
    {
        return item is EquipmentData;
    }

    public bool IsPocketItem(ItemData item)
    {
        return item is EquipmentData;
    }

    public bool IsSeed(ItemData item)
    {
        return item is SeedData;
    }
    #endregion

    public void EquipEmptySlot(ItemData item)
    {
        if (IsEquipment(item))
        {
            selectedToolSlot = new ItemSlotData(item);
        }
        else if (IsSeed(item))
        {
            selectedSeedSlot = new ItemSlotData(item);
        }
        else if (IsPocketItem(item))
        {
            selectedPocketSlot = new ItemSlotData(item);
        }
        else
        {
            selectedInventoryStorageSlot = new ItemSlotData(item);
        }
    }

    private void OnValidate()
    {
        ValidateInventorySlot(selectedToolSlot);
        ValidateInventorySlot(selectedPocketSlot);
        ValidateInventorySlot(selectedSeedSlot);
        ValidateInventorySlot(selectedInventoryStorageSlot);

        ValidateInventorySlots(playerToolsSlots);
        ValidateInventorySlots(playerPocketSlots);
        ValidateInventorySlots(seedsStorageSlots);
        ValidateInventorySlots(inventoryStorageSlots);
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
}
