using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopListing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image itemThumbnail;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    ItemData itemData;

    public void Display(ItemData itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.thumbnail;
        nameText.text = itemData.name;
        costText.text = itemData.cost + PlayerStats.CURRENCY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NewUIManager.Instance.shopListingManager.OpenConfirmationScreen(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NewUIManager.Instance.DisplayItemInfo(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NewUIManager.Instance.DisplayItemInfo(null);
    }
}
