using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue Component")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;

    //The lines to queue during the dialogue sequence
    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;

    bool isTyping = false;
    public bool IsDialoguePlaying => dialoguePanel.activeSelf;

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

    //Initialises the dialogues
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue)
    {
        dialogueQueue = new Queue<DialogueLine>(dialogueLinesToQueue);

        CursorManager.Instance.UIOpened();

        UpdateDialogue();
    }

    //Initialises the dialogue, but with and Action to execute once it finishes
    public void StartDialogue(List<DialogueLine> dialogueLinesToQueue, Action onDialogueEnd)
    {
        StartDialogue(dialogueLinesToQueue);
        this.onDialogueEnd = onDialogueEnd;
    }

    //Cycle through the dialogue lines
    public void UpdateDialogue()
    {
        if (isTyping)
        {
            isTyping = false;
            return;
        }
        //Reset our dialogue text
        dialogueText.text = string.Empty;

        //Check if there are any more lines in the queue
        if(dialogueQueue.Count == 0)
        {
            //If not, end the dialogue
            EndDialogue();
            return;
        }

        //The current dialogue line to put in
        DialogueLine line = dialogueQueue.Dequeue();

        Talk(line.speaker, line.message);
    }

    //Closes the dialogue
    public void EndDialogue()
    {
        //Close the dialogue panel
        dialoguePanel.SetActive(false);

        //Invoke whatever Action queued on dialogue end (if any)
        onDialogueEnd?.Invoke();

        //Reset the Action
        onDialogueEnd = null;

        // Check if the shop UI is still open
        if (NewUIManager.Instance.IsShopOpen())
        {
            Debug.Log("Shop Open");
            CursorManager.Instance.UIOpened();
        }
        else
        {
            CursorManager.Instance.UIClosed();
        }
    }

    public void Talk(string speaker, string message)
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speaker;

        //If there is no speaker, do not show the speaker panel
        speakerText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(speaker));

        // Determine the last character for sound selection
        char lastCharacter = message.Length > 0 ? message[message.Length - 1] : ' ';

        // Remove it only for display if it's R, L, or P
        if (lastCharacter == 'R' || lastCharacter == 'L' || lastCharacter == 'P')
        {
            message = message.Substring(0, message.Length - 1);
        }


        //dialogueText.text = messaage;
        StartCoroutine(TypeText(message, lastCharacter));
    }

    IEnumerator TypeText(string textToType, char lastCharacter)
    {
        isTyping = true;
        char[] charsToType = textToType.ToCharArray();
        SoundManager soundManager = FindObjectOfType<SoundManager>();

        for (int i = 0; i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];

            if (soundManager != null && (i % 3 == 0 || i % 6 == 0))
            {
                int randomIndex = UnityEngine.Random.Range(1, 3);
                string soundToPlay = "";

                if (lastCharacter == 'R')
                {
                    soundToPlay = (i % 6 == 0) ? "ChefTalk" : randomIndex == 1 ? "Speak1" : "Speak2";
                }
                else if (lastCharacter == 'L')
                {
                    soundToPlay = (i % 6 == 0) ? "ConbiniTalk" : randomIndex == 2 ? "Speak2" : "Speak3";
                }
                else if (lastCharacter == 'P')
                {
                    soundToPlay = (i % 6 == 0) ? "SeedStoreTalk" : randomIndex == 1 ? "Speak1" : "Speak3";
                }
                else
                {
                    soundToPlay = "DefaultTalk";
                }

                soundManager.Play(soundToPlay);
            }

            yield return new WaitForSeconds(textSpeed);

            if (!isTyping)
            {
                dialogueText.text = textToType;
                break;
            }
        }

        isTyping = false;
    }
}
