using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NewInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemData itemToDisplay;
    int quantity;

    public Image itemDisplayImage;
    public TextMeshProUGUI quantityText;

    public enum InventoryType
    {
        PlayerTool, PlayerPocket, Seed, Storage
    }

    public InventoryType inventoryType;

    int slotIndex;

    public void Display(ItemSlotData itemSlot)
    {
        itemToDisplay = itemSlot.itemData;
        quantity = itemSlot.quantity;

        //By default, the quantity text shoulld not show
        quantityText.text = "";

        if(itemToDisplay != null)
        {
            itemDisplayImage.sprite = itemToDisplay.thumbnail;

            //Display the Number if stack quanity is more than 1
            if(quantity > 1)
            {
                quantityText.text = quantity.ToString();
            }

            itemDisplayImage.gameObject.SetActive(true);

            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        NewInventoryManager.Instance.InventoryToEquip(slotIndex, inventoryType);
    }

    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NewUIManager.Instance.DisplayItemInfo(itemToDisplay);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NewUIManager.Instance.DisplayItemInfo(null);
    }
}
