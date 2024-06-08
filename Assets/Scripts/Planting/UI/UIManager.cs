using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //Inventory UI
    public GameObject inventoryPanel;

    [Header("Inventory System")]
    public InventorySlot[] toolSlots;
    public InventorySlot[] itemSlots;

    //Info Box
    [Header("Item Name Box")]
    public TextMeshProUGUI itemNameText;

    [Header("Item Description Box")]
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
    }

    public void RenderInventory()
    {
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render Tool Section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render Item Section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);
    }

    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }


    //This will called by the Button for opening the Item Box
    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    //Display Item name and Description
    public void DisplayItemInfo(ItemData data)
    {
        if(data == null)
        {
            itemNameText.text = " ";
            itemDescriptionText.text = " ";

            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }
}
