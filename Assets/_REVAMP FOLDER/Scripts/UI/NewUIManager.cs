using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewUIManager : MonoBehaviour
{
    public static NewUIManager Instance { get; private set; }

    [Header("Inventory System")]
    public NewInventorySlot[] playerToolsSlot;
    public NewInventorySlot[] seedSlot;
    public NewInventorySlot[] storageSlots;

    [Header("Seeds Info Box")]
    public TextMeshProUGUI seedNameText;
    public TextMeshProUGUI seedDescriptionText;

    [Header("Storage Info Box")]
    public TextMeshProUGUI storageNameText;
    public TextMeshProUGUI storageDescriptionText;

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

    private void Start()
    {
        RenderPlayerInventory();
    }

    void RenderPlayerInventory()
    {
        //Get the Inventory PlayerTools from NewInventoryManager
        ItemData[] inventoryPlayerTools = NewInventoryManager.Instance.playerTools;

        //Render the Player Tools section
        RenderInventoryPanel(inventoryPlayerTools, playerToolsSlot);
    }

    public void RenderInventory()
    {
        //Get the Inventory SeedSlot from NewInventoryManager
        ItemData[] inventorySeedSlot = NewInventoryManager.Instance.seedsSlots;

        //Get the Inventory StorageSlot from NewInventoryManager
        ItemData[] inventoryStorageSlot = NewInventoryManager.Instance.storageSlots;

        //Render the Seeds section
        RenderInventoryPanel(inventorySeedSlot, seedSlot);

        //Render the Storage section
        RenderInventoryPanel(inventoryStorageSlot, storageSlots);
    }

    void RenderInventoryPanel(ItemData[] slots, NewInventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            //Displays the Image
            uiSlots[i].Display(slots[i]);
        }
    }

    public void DisplayItemInfo(ItemData data)
    {
        if(data == null)
        {
            seedNameText.text = "";
            seedDescriptionText.text = "";

            storageNameText.text = "";
            storageDescriptionText.text = "";

            return;
        }

        seedNameText.text = data.name;
        seedDescriptionText.text = data.description;

        storageNameText.text = data.name;
        storageDescriptionText.text = data.description;
    }
}
