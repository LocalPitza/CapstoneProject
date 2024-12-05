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

    [Header("Storage UI")]
    [SerializeField] 
    private ItemSlotData[] storageSlots = new ItemSlotData[17];

    [SerializeField] 
    private ItemSlotData equippedStorageSlot = null;

    [Header("Harvested UI")]
    [SerializeField] 
    private ItemSlotData[] harvestedSlots = new ItemSlotData[17];

    [SerializeField] 
    private ItemSlotData equippedHarvestSlot = null;

    public Transform handPoint;

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

            EquipHandSlot(slotToEquip);
        }

        NewUIManager.Instance.RenderInventory();


        /*if(inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            ItemData seedToSelect = harvestedSlots[slotIndex];

            harvestedSlots[slotIndex] = selectedHarvestSlot;

            selectedHarvestSlot = seedToSelect;
        }
        else //Storage Box
        {
            ItemData storageToSelect = storageSlots[slotIndex];

            storageSlots[slotIndex] = selectedStorageSlot;

            selectedStorageSlot = storageToSelect;

            //RenderHand();
        }

        NewUIManager.Instance.RenderInventory();*/
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

        /*if (inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            for (int i = 0; i < harvestedSlots.Length; i++)
            {
                if (harvestedSlots[i] == null)
                {
                    harvestedSlots[i] = selectedHarvestSlot;

                    selectedHarvestSlot = null;
                    break;
                }
            }
        }
        else
        {
            if (selectedStorageSlot != null)
            {
                for (int i = 0; i < storageSlots.Length; i++)
                {
                    if (storageSlots[i] == null)
                    {
                        storageSlots[i] = selectedStorageSlot;
                        selectedStorageSlot = null;
                        break;
                    }
                }
            }
        }
        
        NewUIManager.Instance.RenderInventory();*/
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

}
