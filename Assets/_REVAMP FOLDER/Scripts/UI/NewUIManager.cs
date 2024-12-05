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
    public Image toolEquippedIcon;
    public Image harvestEquippedIcon;

    [Header("Equip Slots")]
    public NewHandInventorySlot storageEquippedSlot;
    public NewHandInventorySlot harvestEquippedSlot;

    [Header("Inventory System")]
    public NewInventorySlot[] storageSlots;
    public NewInventorySlot[] harvestSlot;

    [Header("Info Box")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

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
        for (int j = 0; j < harvestSlot.Length; j++)
        {
            harvestSlot[j].AssignIndex(j);
        }

        for(int k = 0; k < storageSlots.Length; k++)
        {
            storageSlots[k].AssignIndex(k);
        }
    }

    public void RenderInventory()
    {
        //Get the Inventory SeedSlot from NewInventoryManager
        ItemData[] inventorySeedSlot = NewInventoryManager.Instance.harvestedSlots;

        //Get the Inventory StorageSlot from NewInventoryManager
        ItemData[] inventoryStorageSlot = NewInventoryManager.Instance.storageSlots;

        //Render the Seeds section
        RenderInventoryPanel(inventorySeedSlot, harvestSlot);

        //Render the Storage section
        RenderInventoryPanel(inventoryStorageSlot, storageSlots);

        harvestEquippedSlot.Display(NewInventoryManager.Instance.selectedHarvest);
        storageEquippedSlot.Display(NewInventoryManager.Instance.selectedStorage);

        //Get ToolEquip from NewInventoryManager
        ItemData selectedStorage = NewInventoryManager.Instance.selectedStorage;
        ItemData selectedHarvest = NewInventoryManager.Instance.selectedHarvest;

        if (selectedStorage != null)
        {
            toolEquippedIcon.sprite = selectedStorage.thumbnail;
            toolEquippedIcon.gameObject.SetActive(true);

            return;
        }

        if(selectedHarvest != null)
        {
            harvestEquippedIcon.sprite = selectedHarvest.thumbnail;
            harvestEquippedIcon.gameObject.SetActive(true);

            return;
        }

        toolEquippedIcon.gameObject.SetActive(false);
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
        nameText.text = "";
        descriptionText.text = "";

        if (data == null)
        {
            return;
        }

        switch (data.itemType)
        {
            case ItemType.HarvestItem:
                // Show in Seed UI only
                nameText.text = data.name;
                descriptionText.text = data.description;
                break;

            case ItemType.StorageItem:
                // Show in Storage UI only
                nameText.text = data.name;
                descriptionText.text = data.description;
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
