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

    //The Full list of Items
    public ItemIndex itemIndex;

    [Header("Storage UI")]
    [SerializeField]
    private ItemSlotData equippedStorageSlot = null;

    [SerializeField] 
    private ItemSlotData[] storageSlots = new ItemSlotData[17];

    [Header("Harvested UI")]
    [SerializeField]
    private ItemSlotData equippedHarvestSlot = null;

    [SerializeField] 
    private ItemSlotData[] harvestedSlots = new ItemSlotData[17];

    public Transform handPoint;

    public void LoadInventory(ItemSlotData[] storageSlots, ItemSlotData equippedStorageSlot, ItemSlotData[] harvestedSlots, ItemSlotData equippedHarvestSlot)
    {
        this.storageSlots = storageSlots;
        this.equippedStorageSlot = equippedStorageSlot;

        this.harvestedSlots = harvestedSlots;
        this.equippedHarvestSlot = equippedHarvestSlot;

        NewUIManager.Instance.RenderInventory();
    }

    //Equipping
    public void InventoryToEquip(int slotIndex, NewInventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handToEquip = equippedStorageSlot;
        ItemSlotData[] inventoryToAlter = storageSlots;

        if(inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            handToEquip = equippedHarvestSlot;
            inventoryToAlter = harvestedSlots;
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            handToEquip.AddQuantity(slotToAlter.quantity);

            slotToAlter.Empty();
        }
        else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            if (slotToEquip.IsEmpty())
            {
                handToEquip.Empty();
            }
            else
            {
                EquipHandSlot(slotToEquip);
            }
        }

        NewUIManager.Instance.RenderInventory();
    }

    public void EquipToInventory(NewInventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = equippedStorageSlot;
        ItemSlotData[] inventoryToAlter = storageSlots;

        if (inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            handSlot = equippedHarvestSlot;
            inventoryToAlter = harvestedSlots;
        }

        if(!StackItemToInventory(handSlot, inventoryToAlter))
        {
            for(int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);

                    handSlot.Empty();

                    break;
                }
                
            }
        }

        NewUIManager.Instance.RenderInventory();
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for(int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                inventoryArray[i].AddQuantity(itemSlot.quantity);

                itemSlot.Empty();

                return true;
            }
        }
        return false;
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }

        if(SlotEquipped(NewInventorySlot.InventoryType.Harvest))
        {
            Instantiate(GetEquippedSlotItem(NewInventorySlot.InventoryType.Harvest).gameModel, handPoint);
        }
    }

    public ItemData GetEquippedSlotItem(NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            return equippedHarvestSlot.itemData;
        }
        return equippedStorageSlot.itemData;
    }

    public ItemSlotData GetEquippedSlot(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            return equippedHarvestSlot;
        }
        return equippedStorageSlot;
    }

    public ItemSlotData[] GetInventorySlots(NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            return harvestedSlots;
        }
        return storageSlots;
    }
    
    public bool SlotEquipped(NewInventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            return !equippedHarvestSlot.IsEmpty();
        }
        return !equippedStorageSlot.IsEmpty();
    }

    public bool IsTool(ItemData item)
    {
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;
        }

        SeedData seed = item as SeedData;
        return seed != null;
    }

    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
        {
            equippedStorageSlot = new ItemSlotData(item);
        }
        else
        {
            equippedHarvestSlot = new ItemSlotData(item);
        }
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;
        if (IsTool(item))
        {
            equippedStorageSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedHarvestSlot = new ItemSlotData(itemSlot);
        }
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty())
        {
            Debug.LogError("No more to consume");
            return;
        }

        itemSlot.Remove();

        //Refreshes the Inventory
        NewUIManager.Instance.RenderInventory();
    }

    #region Inventory Slot Validation

    private void OnValidate()
    {
        ValidateInventorySlot(equippedStorageSlot);
        ValidateInventorySlot(equippedHarvestSlot);

        ValidateInventorySlots(storageSlots);
        ValidateInventorySlots(harvestedSlots);
    }

    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
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
