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
    public ItemData[] storageSlots = new ItemData[17];
    public ItemData selectedStorage = null;

    [Header("Harvested UI")]
    public ItemData[] harvestedSlots = new ItemData[17];
    public ItemData selectedHarvest = null;

    public Transform handPoint;

    //Equipping
    public void InventoryToEquip(int slotIndex, NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            ItemData seedToSelect = harvestedSlots[slotIndex];

            harvestedSlots[slotIndex] = selectedHarvest;

            selectedHarvest = seedToSelect;
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
        if (inventoryType == NewInventorySlot.InventoryType.Harvest)
        {
            for (int i = 0; i < harvestedSlots.Length; i++)
            {
                if (harvestedSlots[i] == null)
                {
                    harvestedSlots[i] = selectedHarvest;

                    selectedHarvest = null;
                    break;
                }
            }
        }
        else
        {
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
        }
        
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
