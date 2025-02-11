using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopListingManager : MonoBehaviour
{
    public GameObject shopParent;
    public GameObject shopListing;
    public Transform listingGrid;

    ItemData itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    public GameObject confirmationScreen;
    public Image productThumbnail;
    public TextMeshProUGUI confirmationPrompt;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI costCalculationText;
    public Button purchaseButton;

    public void RenderShop(List<ItemData> shopItems)
    {
        //Resets the listings if there was a previous one
        if(listingGrid.childCount > 0)
        {
            foreach(Transform child in listingGrid)
            {
                Destroy(child.gameObject);
            }
        }

        //Create a new listing for every item
        foreach(ItemData shopItem in  shopItems)
        {
            GameObject listingGameObject = Instantiate(shopListing, listingGrid);

            listingGameObject.GetComponent<ShopListing>().Display(shopItem);
        }
    }

    public void OpenConfirmationScreen(ItemData itemData)
    {
        itemToBuy = itemData;
        productThumbnail.sprite = itemToBuy.thumbnail;
        quantity = 1;
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        confirmationScreen.SetActive(true);

        confirmationPrompt.text = $"Buy {itemToBuy.name}?";

        quantityText.text = "x" + quantity;

        // Check if the item is a FoodData type and requires ingredients
        FoodData food = itemToBuy as FoodData;
        if (food != null && food.needIngredient)
        {
            // Check if the player has enough ingredients
            bool hasIngredients = CookManager.Instance.HasEnoughIngredients(food.requiredIngredients, quantity);
            purchaseButton.interactable = hasIngredients;

            if (!hasIngredients)
            {
                costCalculationText.text = "Not enough ingredients!";
                return;
            }
        }
        else
        {
            // Normal money-based purchase
            int cost = itemToBuy.cost * quantity;
            int playerMoneyLeft = PlayerStats.Money - cost;

            if (playerMoneyLeft < 0)
            {
                costCalculationText.text = "Insufficient funds.";
                purchaseButton.interactable = false;
                return;
            }

            costCalculationText.text = $"{PlayerStats.Money} > {playerMoneyLeft}";
            purchaseButton.interactable = true;
        }

        /*int cost = itemToBuy.cost * quantity;

        int playerMoneyLeft = PlayerStats.Money - cost;

        if(playerMoneyLeft < 0)
        {
            costCalculationText.text = "Insufficient funds.";
            purchaseButton.interactable = false;
            return;
        }

        purchaseButton.interactable = true;

        costCalculationText.text = $"{PlayerStats.Money} > {playerMoneyLeft} ";*/
    }

    //Accessed by the Add Quantity Button
    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }

    //Accessed by the Reduce Quantity Button
    public void SubtractQuantity()
    {
        if(quantity > 1)
        {
            quantity--;
        }
        RenderConfirmationScreen();
    }

    public void ConfirmPurchase()
    {
        Shop.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmationScreen.SetActive(false);
    }

    public void CloseShop()
    {
        shopParent.SetActive(false);
        PlayerMove.isUIOpen = false;
    }
}
