using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialMessage
{
    public string message;
    public GameObject associatedObject;
    public int objectActiveForMessages = 1; // How many messages this object stays active for
}

public class TutorialPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private TextMeshProUGUI continueButtonText;
    [SerializeField] private GameObject finishIndicator; // Optional: shows when all messages are done

    [Header("Tutorial Content")]
    [SerializeField] private List<TutorialMessage> tutorialMessages = new List<TutorialMessage>();
    [SerializeField] private bool disableAfterCompletion = true;
    [SerializeField] private string continueButtonTextDefault = "Continue";
    [SerializeField] private string continueButtonTextFinal = "Got it!";

    [Header("Input Settings")]
    [SerializeField] private KeyCode continueKey = KeyCode.Space;
    [SerializeField] private bool allowKeyboardInput = true;

    private int currentMessageIndex = 0;
    private bool isPlayerInTrigger = false;
    private GameObject currentActiveGameObject = null;
    private int messagesRemainingForCurrentObject = 0;

    private void Start()
    {
        // Hide the popup initially
        popupPanel.SetActive(false);
        
        if (finishIndicator != null)
        {
            finishIndicator.SetActive(false);
        }

        // Disable all message GameObjects at start
        foreach (var msg in tutorialMessages)
        {
            if (msg.associatedObject != null) 
                msg.associatedObject.SetActive(false);
        }

        // Set up button click event
        continueButton.onClick.AddListener(ShowNextMessage);
        
        // Set initial button text
        if (continueButtonText != null)
        {
            continueButtonText.text = continueButtonTextDefault;
        }
    }

    private void Update()
    {
        // Allow keyboard input to continue
        if (isPlayerInTrigger && allowKeyboardInput && Input.GetKeyDown(continueKey))
        {
            ShowNextMessage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            currentMessageIndex = 0;
            messagesRemainingForCurrentObject = 0;
            ShowCurrentMessage();
            popupPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            popupPanel.SetActive(false);
            
            // Disable any active GameObject when leaving
            if (currentActiveGameObject != null)
            {
                currentActiveGameObject.SetActive(false);
                currentActiveGameObject = null;
            }
        }
    }

    private void ShowCurrentMessage()
    {
        if (currentMessageIndex < tutorialMessages.Count)
        {
            var currentMessage = tutorialMessages[currentMessageIndex];
            messageText.text = currentMessage.message;
            
            // Update button text if this is the last message
            if (continueButtonText != null)
            {
                continueButtonText.text = (currentMessageIndex == tutorialMessages.Count - 1) 
                    ? continueButtonTextFinal 
                    : continueButtonTextDefault;
            }

            // Handle associated GameObjects
            if (messagesRemainingForCurrentObject <= 0 && currentMessage.associatedObject != null)
            {
                // Disable previous GameObject if exists
                if (currentActiveGameObject != null)
                {
                    currentActiveGameObject.SetActive(false);
                }
                
                // Enable new GameObject and set duration
                currentActiveGameObject = currentMessage.associatedObject;
                currentActiveGameObject.SetActive(true);
                messagesRemainingForCurrentObject = currentMessage.objectActiveForMessages;
            }
            
            // Decrement the counter for the current object
            messagesRemainingForCurrentObject--;
        }
    }

    public void ShowNextMessage()
    {
        currentMessageIndex++;

        if (currentMessageIndex < tutorialMessages.Count)
        {
            ShowCurrentMessage();
        }
        else
        {
            // All messages shown
            if (disableAfterCompletion)
            {
                popupPanel.SetActive(false);
            }
            else
            {
                if (finishIndicator != null)
                {
                    finishIndicator.SetActive(true);
                }
                continueButton.gameObject.SetActive(false);
            }
            
            // Disable any active GameObject when tutorial completes
            if (currentActiveGameObject != null)
            {
                currentActiveGameObject.SetActive(false);
                currentActiveGameObject = null;
            }
        }
    }

    // Call this if you want to reset the tutorial for reuse
    public void ResetTutorial()
    {
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
        continueButton.gameObject.SetActive(true);
        
        if (finishIndicator != null)
        {
            finishIndicator.SetActive(false);
        }
        
        if (continueButtonText != null)
        {
            continueButtonText.text = continueButtonTextDefault;
        }

        // Disable any active GameObject when resetting
        if (currentActiveGameObject != null)
        {
            currentActiveGameObject.SetActive(false);
            currentActiveGameObject = null;
        }
    }

    // Public method to manually trigger the tutorial
    public void StartTutorial()
    {
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
        ShowCurrentMessage();
        popupPanel.SetActive(true);
        isPlayerInTrigger = true;
    }
}
