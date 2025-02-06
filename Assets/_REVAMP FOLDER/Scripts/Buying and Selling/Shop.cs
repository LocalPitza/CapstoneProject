using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private InteractMessage interactMessage;
    public List<ItemData> shopItems;

    //[Header("Dialogues")]
    //public List<DialogueLine> dialogueOnShopOpen;

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
            StartConversation();
        }
    }

    void StartConversation()
    {
        if (npcData != null)
        {
            bool hasMetBefore = PlayerPrefs.GetInt(npcID, 0) == 1;

            if (!hasMetBefore)
            {
                // First time meeting the NPC
                DialogueManager.Instance.StartDialogue(npcData.onFirstMeet, AfterFirstMeet);
            }
            else
            {
                // Already met before, use default dialogue
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
