using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private InteractMessage interactMessage;
    public List<ItemData> shopItems;

    void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
    }

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if(PlayerStats.Money >= totalCost)
        {
            PlayerStats.Spend(totalCost);

            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            //Send the purchased Item to the player's inventory
            NewInventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    }

    void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            NewUIManager.Instance.OpenShop(shopItems);
            PlayerMove.isUIOpen = true;
        }
    }
}
