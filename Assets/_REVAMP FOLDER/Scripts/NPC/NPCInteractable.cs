using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    private InteractMessage interactMessage;
    [SerializeField] GameObject testObject;
    [SerializeField] NPCData npcData;

    [Header("Want Item")]
    [SerializeField] ItemData wantItem;

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

        // Check if testObject is already activated or it's a special day
        if (!isActivated && specialDialogue != null)
        {
            // Special day dialogue triggers testObject activation
            DialogueManager.Instance.StartDialogue(specialDialogue, () =>
            {
                ActivateTestObject();
                DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
            });
        }
        else
        {
            // If object is already activated or no special dialogue, go to default
            DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
        }
    }

    void AfterFirstMeet()
    {
        // Mark NPC as met and save
        PlayerPrefs.SetInt(npcID, 1);
        PlayerPrefs.Save();

        // Transition to default dialogue after the first meeting
        DialogueManager.Instance.StartDialogue(npcData.defaultDialogue);
    }

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
