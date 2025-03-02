using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private InteractMessage interactMessage;
    [SerializeField] GameObject testObject;
    [SerializeField] NPCData npcData;

    private string npcID;
    private string activatedKey;

    private void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
        npcID = "NPC_" + gameObject.name;
        activatedKey = npcID + "_Activated";

        // Check if the object was previously activated
        if (PlayerPrefs.GetInt(activatedKey, 0) == 1)
        {
            ActivateTestObject();
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
        int currentDay = TimeManager.Instance.CurrentDay;
        bool hasMetBefore = PlayerPrefs.GetInt(npcID, 0) == 1;
        bool isActivated = PlayerPrefs.GetInt(activatedKey, 0) == 1;

        var specialDialogue = npcData.GetDialogueForDay(currentDay);

        // Check if it's the first time meeting the NPC
        if (!hasMetBefore)
        {
            DialogueManager.Instance.StartDialogue(npcData.onFirstMeet, AfterFirstMeet);
            return;
        }

        // Check if it's a special day
        if (!isActivated && specialDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(specialDialogue, () =>
            {
                if (TryGiveWantedItem())
                {
                    DialogueManager.Instance.StartDialogue(npcData.thankYouDialogue);
                    ActivateTestObject();
                }
                else
                {
                    DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
                }
            });
            return;
        }

        // If object is already activated or no special dialogue, go to default
        DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
    }

    bool TryGiveWantedItem()
    {
        if (npcData.wantItem == null) return false;

        var harvestedSlots = NewInventoryManager.Instance.GetInventorySlots(NewInventorySlot.InventoryType.Harvest);

        for (int i = 0; i < harvestedSlots.Length; i++)
        {
            var slot = harvestedSlots[i];
            if (slot.itemData == npcData.wantItem && slot.quantity > 0)
            {
                // Remove one item
                slot.Remove();
                harvestedSlots[i] = slot;
                NewUIManager.Instance.RenderInventory();
                return true;
            }
        }

        return false;
    }

    void AfterFirstMeet()
    {
        // Mark NPC as met and save
        PlayerPrefs.SetInt(npcID, 1);
        PlayerPrefs.Save();

        // Transition to default dialogue after the first meeting
        DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
    }

    /*IEnumerator ActivateTestObjectDelayed()
    {
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds
        ActivateTestObject();
    }*/

    void ActivateTestObject()
    {
        if (testObject != null && !testObject.activeSelf)
        {
            testObject.SetActive(true);
            PlayerPrefs.SetInt(activatedKey, 1);
            PlayerPrefs.Save();
            Debug.Log("testObject activated and will remain active permanently.");
        }
    }


}
