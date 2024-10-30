using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewUIManager : MonoBehaviour, ITimeTracker
{
    public static NewUIManager Instance { get; private set; }

    [Header("In-Game Time")]
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    [Header("Player Equipped Slot")]
    public Image toolEquippedSlot;

    [Header("Equip Slots")]
    public NewHandInventorySlot playerToolEquippedSlot;
    public NewHandInventorySlot seedEquippedSlot;
    public NewHandInventorySlot storageEquippedSlot;

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
        RenderInventory();
        AssignSlotIndexes();

        TimeManager.Instance.RegisterTracker(this);
    }

    public void AssignSlotIndexes()
    {
        for(int i = 0; i < playerToolsSlot.Length; i++)
        {
            playerToolsSlot[i].AssignIndex(i);
        }

        for(int j = 0; j < seedSlot.Length; j++)
        {
            seedSlot[j].AssignIndex(j);
        }

        for(int k = 0; k < storageSlots.Length; k++)
        {
            storageSlots[k].AssignIndex(k);
        }
    }

    public void RenderInventory()
    {
        //Get the Inventory PlayerTools from NewInventoryManager
        ItemData[] inventoryPlayerTools = NewInventoryManager.Instance.playerTools;

        //Get the Inventory SeedSlot from NewInventoryManager
        ItemData[] inventorySeedSlot = NewInventoryManager.Instance.seedsSlots;

        //Get the Inventory StorageSlot from NewInventoryManager
        ItemData[] inventoryStorageSlot = NewInventoryManager.Instance.storageSlots;

        //Render the Player Tools section
        RenderInventoryPanel(inventoryPlayerTools, playerToolsSlot);

        //Render the Seeds section
        RenderInventoryPanel(inventorySeedSlot, seedSlot);

        //Render the Storage section
        RenderInventoryPanel(inventoryStorageSlot, storageSlots);

        playerToolEquippedSlot.Display(NewInventoryManager.Instance.selectedTool);
        seedEquippedSlot.Display(NewInventoryManager.Instance.selectedSeed);
        storageEquippedSlot.Display(NewInventoryManager.Instance.selectedStorage);

        //Get ToolEquip from NewInventoryManager
        ItemData selectedTool = NewInventoryManager.Instance.selectedTool;

        if (selectedTool != null)
        {
            toolEquippedSlot.sprite = selectedTool.thumbnail;
            toolEquippedSlot.gameObject.SetActive(true);

            return;
        }

        toolEquippedSlot.gameObject.SetActive(false);
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
        seedNameText.text = "";
        seedDescriptionText.text = "";
        storageNameText.text = "";
        storageDescriptionText.text = "";

        if (data == null)
        {
            return;
        }

        switch (data.itemType)
        {
            case ItemType.Seed:
                // Show in Seed UI only
                seedNameText.text = data.name;
                seedDescriptionText.text = data.description;
                break;

            case ItemType.StorageItem:
                // Show in Storage UI only
                storageNameText.text = data.name;
                storageDescriptionText.text = data.description;
                break;

            // Optionally, handle other types like tools or other items
            case ItemType.Tool:
                // Add tool UI handling if needed
                break;
        }
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = " AM";

        if(hours > 12)
        {
            prefix = " PM";
            hours -= 12;
        }

        timeText.text = hours.ToString("00") + ":" + minutes.ToString("00") + prefix;

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season +" "+ day + " (" + dayOfTheWeek + ")";
    }
}
