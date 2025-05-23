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
    public GameObject itemInfoBox;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;

    [Header("Player Stats")]
    public TextMeshProUGUI moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    [Header("Stamina")]
    public Slider energyBar;
    public int staminaCount;

    [Header("Hunger Bar")]
    public Slider hungerBar;
    public int hungerCount;

    [Header("Warning Indicator")]
    public Image warningIndicator;
    public Sprite hungerWarningSprite;
    public Sprite energyWarningSprite;
    public float blinkInterval = 0.5f;

    private Coroutine warningBlinkCoroutine;
    private Coroutine energyFlickerCoroutine;
    private Coroutine hungerFlickerCoroutine;

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
        PlayerStats.RestoreStamina(100);
        RenderInventory();
        AssignSlotIndexes();
        RenderPlayerStats();
        DisplayItemInfo(null);

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
        // Show ItemInfoBox if the inventory is open OR the shop is open
        if (!FindObjectOfType<OpenBackpack>().IsBackpackOpen() && !IsShopOpen())
        {
            itemInfoBox.SetActive(false);
            return;
        }

        //If data is null, reset
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemInfoBox.SetActive(false);
            return;
        }

        itemInfoBox.SetActive(true);
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }
    #endregion

    public void TriggerYesNoPrompt(string message, System.Action onYesCallBack)
    {
        yesNoPrompt.gameObject.SetActive(true);

        CursorManager.Instance.UIOpened();

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
        staminaCount = PlayerStats.Stamina;
        hungerCount = PlayerStats.Hunger;
        ChangeStatsUI();
    }

    public void ChangeStatsUI()
    {
        if (energyBar != null)
        {
            energyBar.maxValue = 100;
            energyBar.value = staminaCount;
            UpdateBarColor(energyBar, staminaCount, ref energyFlickerCoroutine);
        }

        if (hungerBar != null)
        {
            hungerBar.maxValue = 100;
            hungerBar.value = hungerCount;
            UpdateBarColor(hungerBar, hungerCount, ref hungerFlickerCoroutine);
        }

        UpdateWarningIndicator();
    }

    private void UpdateBarColor(Slider bar, int value, ref Coroutine flickerCoroutine)
    {
        Image barImage = bar.fillRect.GetComponent<Image>();

        if (value > 75)
        {
            barImage.color = Color.green;
        }
        else if (value > 50)
        {
            barImage.color = Color.yellow;
        }
        else if (value > 20)
        {
            barImage.color = new Color(1f, 0.5f, 0f); // Orange
        }
        else
        {
            barImage.color = Color.red;

            if (flickerCoroutine == null)
                flickerCoroutine = StartCoroutine(FlickerBar(bar, barImage));
        }

        if (value > 20 && flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
            barImage.color = value > 50 ? Color.yellow : new Color(1f, 0.5f, 0f); // Reset color
        }
    }

    private IEnumerator FlickerBar(Slider bar, Image barImage)
    {
        while (true)
        {
            barImage.color = new Color(1f, 0f, 0f, 0.2f); // Semi-transparent red
            yield return new WaitForSeconds(0.2f);
            barImage.color = Color.red;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.RenderShop(shopItems);

        CursorManager.Instance.UIOpened();
    }

    public bool IsShopOpen()
    {
        return shopListingManager.shopParent.activeSelf;
    }

    private void UpdateWarningIndicator()
    {
        if (hungerCount <= 20)
        {
            SetWarningIndicator(hungerWarningSprite);
        }
        else if (staminaCount <= 20)
        {
            SetWarningIndicator(energyWarningSprite);
        }
        else
        {
            ClearWarningIndicator();
        }
    }

    private void SetWarningIndicator(Sprite warningSprite)
    {
        if (warningBlinkCoroutine != null)
        {
            StopCoroutine(warningBlinkCoroutine);
        }

        warningIndicator.sprite = warningSprite;
        warningIndicator.gameObject.SetActive(true);
        warningBlinkCoroutine = StartCoroutine(BlinkWarningIndicator());

        // Play warning sound here
    }

    private void ClearWarningIndicator()
    {
        if (warningBlinkCoroutine != null)
        {
            StopCoroutine(warningBlinkCoroutine);
            warningBlinkCoroutine = null;
        }

        warningIndicator.gameObject.SetActive(false);
    }

    private IEnumerator BlinkWarningIndicator()
    {
        while (true)
        {
            warningIndicator.enabled = !warningIndicator.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
