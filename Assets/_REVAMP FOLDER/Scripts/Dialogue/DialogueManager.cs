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

    //The lines to queue during the dialogue sequence
    Queue<DialogueLine> dialogueQueue;
    Action onDialogueEnd = null;

    bool isTyping = false;

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
    }

    public void Talk(string speaker, string messaage)
    {
        dialoguePanel.SetActive(true);

        speakerText.text = speaker;

        //If there is no speaker, do not show the speaker panel
        speakerText.transform.parent.gameObject.SetActive(speaker != "");

        //dialogueText.text = messaage;
        StartCoroutine(TypeText(messaage));
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;

        char[] charsToType = textToType.ToCharArray();
        for(int i = 0; i < charsToType.Length; i++)
        {
            dialogueText.text += charsToType[i];
            yield return new WaitForEndOfFrame();

            //Skip the typing sequence and just show the full text
            if (!isTyping)
            {
                dialogueText.text = textToType;

                //Break out from the loop
                break;
            }
        }

        //Typing sequence complete
        isTyping = false;
    }
}
