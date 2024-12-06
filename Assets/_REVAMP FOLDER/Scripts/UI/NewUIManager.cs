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

    [Header("Storage")]
    public NewHandInventorySlot storageEquippedSlot;
    public NewInventorySlot[] storageSlots;

    [Header("Harvest")]
    public NewHandInventorySlot harvestEquippedSlot;
    public NewInventorySlot[] harvestSlot;

    [Header("Info Box")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

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
        for(int k = 0; k < storageSlots.Length; k++)
        {
            storageSlots[k].AssignIndex(k);
            harvestSlot[k].AssignIndex(k);
        }
    }

    public void RenderInventory()
    {
        ItemSlotData[] inventoryStorageSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Storage);
        ItemSlotData[] inventoryHarvestSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest);

        //Render the Storage section
        RenderInventoryPanel(inventoryStorageSlots, storageSlots);

        //Render the Seeds section
        RenderInventoryPanel(inventoryHarvestSlots, harvestSlot);

        storageEquippedSlot.Display(NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Storage));
        harvestEquippedSlot.Display(NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest));

        //Get ToolEquip from NewInventoryManager
        ItemData selectedStorage = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Storage);
        ItemData selectedHarvest = NewInventoryManager.Instance.GetEquippedSlotItem(NewInventorySlot.InventoryType.Harvest);

        // Update the tool equipped icon
        if (selectedStorage != null)
        {
            toolEquippedIcon.sprite = selectedStorage.thumbnail;
            toolEquippedIcon.gameObject.SetActive(true);
        }
        else
        {
            toolEquippedIcon.gameObject.SetActive(false);
        }

        // Update the harvest equipped icon
        if (selectedHarvest != null)
        {
            harvestEquippedIcon.sprite = selectedHarvest.thumbnail;
            harvestEquippedIcon.gameObject.SetActive(true);
        }
        else
        {
            harvestEquippedIcon.gameObject.SetActive(false);
        }

    }

    void RenderInventoryPanel(ItemSlotData[] slots, NewInventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            //Displays the Image
            uiSlots[i].Display(slots[i]);
        }
    }

    public void DisplayItemInfo(ItemData data)
    {
        //If data is null, reset
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
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
