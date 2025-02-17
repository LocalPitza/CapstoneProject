using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private InteractMessage interactMessage;
    public List<ItemData> shopItems;

    [Header("NPC Data")]
    public NPCData npcData;

    private string npcID;

    void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
        npcID = "NPC_" + gameObject.name;
    }

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        // Check if the item is a FoodData
        FoodData food = item as FoodData;
        bool hasRequiredIngredients = true;

        // Check and consume ingredients if needed
        if (food != null && food.needIngredient)
        {
            hasRequiredIngredients = CookManager.Instance.ConsumeIngredients(food.requiredIngredients, quantity);
            if (!hasRequiredIngredients)
            {
                Debug.Log("Missing required ingredients!");
                return;
            }
        }

        // Check for payment if required
        if (food != null && food.requirePayment)
        {
            if (PlayerStats.Money < totalCost)
            {
                Debug.Log("Not enough money!");
                return;
            }
            PlayerStats.Spend(totalCost);
        }
        else
        {
            //For Buying the Seeds
            if (PlayerStats.Money < totalCost)
            {
                Debug.Log("Not enough money!");
                return;
            }
            PlayerStats.Spend(totalCost);
        }

        // Proceed with giving the item
        ItemSlotData purchasedItem = new ItemSlotData(item, quantity);
        NewInventoryManager.Instance.ShopToInventory(purchasedItem);

        Debug.Log("Purchase successful!");
    }

    void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            StartConversation();
        }
    }

    void StartConversation()
    {
        if (npcData != null)
        {
            int currentDay = TimeManager.Instance.GetGameTimeStamp().day;
            bool hasMetBefore = PlayerPrefs.GetInt(npcID, 0) == 1;

            // Check if there is a special dialogue for this day
            List<DialogueLine> specialDialogue = npcData.GetDialogueForDay(currentDay);

            if (!hasMetBefore)
            {
                DialogueManager.Instance.StartDialogue(npcData.onFirstMeet, AfterFirstMeet);
            }
            else if (specialDialogue != null)
            {
                DialogueManager.Instance.StartDialogue(specialDialogue, ShopOpen);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(npcData.defaultDialogue, ShopOpen);
            }

            PlayerMove.isUIOpen = true;
        }
    }

    void AfterFirstMeet()
    {
        // Mark this NPC as met
        PlayerPrefs.SetInt(npcID, 1);
        PlayerPrefs.Save();

        // Proceed to shop opening after first conversation
        ShopOpen();
    }

    void ShopOpen()
    {
        NewUIManager.Instance.OpenShop(shopItems);
        PlayerMove.isUIOpen = true;
    }
}
