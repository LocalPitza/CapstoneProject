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
    public ItemData[] playerTools = new ItemData[5];
    public ItemData selectedTool = null;

    [Header("Player Pocket")]
    public ItemData[] playerPocket = new ItemData[5];
    public ItemData selectedPocket = null;
    public Transform handPoint;

    [Header("Seeds UI")]
    public ItemData[] seedsSlots = new ItemData[5];
    public ItemData selectedSeed = null;

    [Header("Storage UI")]
    public ItemData[] storageSlots = new ItemData[15];
    public ItemData selectedStorage = null;

    //Equipping
    public void InventoryToEquip(int slotIndex, NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            ItemData playerToolToEquip = playerTools[slotIndex];

            playerTools[slotIndex] = selectedTool;

            selectedTool = playerToolToEquip;

        }
        else if(inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            ItemData playerPocketToEquip = playerPocket[slotIndex];

            playerPocket[slotIndex] = selectedPocket;

            selectedPocket = playerPocketToEquip;

            RenderHand();
        }
        else if(inventoryType == NewInventorySlot.InventoryType.Seed)
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
        }

        NewUIManager.Instance.RenderInventory();
    }

    public void EquipToInventory(NewInventorySlot.InventoryType inventoryType)
    {
        if(inventoryType == NewInventorySlot.InventoryType.PlayerTool)
        {
            for(int i = 0; i < playerTools.Length; i++)
            {
                if(playerTools[i] == null)
                {
                    playerTools[i] = selectedTool;

                    selectedTool = null;
                    break;
                }
            }
        }
        else if (inventoryType == NewInventorySlot.InventoryType.PlayerPocket)
        {
            for (int i = 0; i < playerPocket.Length; i++)
            {
                if (playerPocket[i] == null)
                {
                    playerPocket[i] = selectedPocket;

                    selectedPocket = null;
                    break;
                }
            }

            RenderHand();
        }
        else if (inventoryType == NewInventorySlot.InventoryType.Seed)
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

        NewUIManager.Instance.RenderInventory();
    }

    public void RenderHand()
    {
        if(handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        if(selectedPocket != null)
        {
            Instantiate(selectedPocket.harvestableModel, handPoint);
        }
    }
}
