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
    public TextMeshProUGUI toolQuantityText;

    public Image harvestEquippedIcon;
    public TextMeshProUGUI harvestQuantityText;

    [Header("Storage")]
    public NewHandInventorySlot storageEquippedSlot;
    public NewInventorySlot[] storageSlots;

    [Header("Harvest")]
    public NewHandInventorySlot harvestEquippedSlot;
    public NewInventorySlot[] harvestSlot;

    [Header("Info Box")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;

    [Header("Player Stats")]
    public TextMeshProUGUI moneyText;

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
        RenderPlayerStats();

        TimeManager.Instance.RegisterTracker(this);
    }

    #region SlotIndexes
    public void AssignSlotIndexes()
    {
        for(int k = 0; k < storageSlots.Length; k++)
        {
            storageSlots[k].AssignIndex(k);
            harvestSlot[k].AssignIndex(k);
        }
    }
    #endregion

    #region RenderInventory
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

        //By default, quantity text is empty
        toolQuantityText.text = "";
        harvestQuantityText.text = "";

        // Update the tool equipped icon
        if (selectedStorage != null)
        {
            toolEquippedIcon.sprite = selectedStorage.thumbnail;
            toolEquippedIcon.gameObject.SetActive(true);

            int quantity = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Storage).quantity;
            if(quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
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

            int quantity = NewInventoryManager.Instance.GetEquippedSlot(NewInventorySlot.InventoryType.Harvest).quantity;
            if (quantity > 1)
            {
                harvestQuantityText.text = quantity.ToString();
            }
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
    #endregion

    #region Display Item Info
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
    #endregion

    public void TriggerYesNoPrompt(string message, System.Action onYesCallBack)
    {
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallBack);
    }

    #region Time
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
    #endregion

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.Money + PlayerStats.CURRENCY;
    }
}
