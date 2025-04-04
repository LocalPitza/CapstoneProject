using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject finishIndicator;

    [Header("Tutorial Content")]
    [SerializeField] private List<TutorialMessage> tutorialMessages = new List<TutorialMessage>();
    [SerializeField] private bool disableAfterCompletion = true;
    [SerializeField] private string continueButtonTextDefault = "Continue";
    [SerializeField] private string continueButtonTextFinal = "Got it!";
    [SerializeField] private string tutorialID = "default_tutorial";

    [Header("Input Settings")]
    [SerializeField] private KeyCode continueKey = KeyCode.Space;
    [SerializeField] private bool allowKeyboardInput = true;
    [SerializeField] private bool persistCompletionState = true;

    private int currentMessageIndex = 0;
    private bool isPlayerInTrigger = false;
    private GameObject currentActiveGameObject = null;
    private int messagesRemainingForCurrentObject = 0;
    private bool tutorialCompleted = false;

    private void Awake()
    {
        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset tutorial state when a new scene loads
        ForceCloseTutorial();
    }

    private void Start()
    {
        // Check if tutorial was already completed
        if (persistCompletionState)
        {
            tutorialCompleted = PlayerPrefs.GetInt("TutorialCompleted_" + tutorialID, 0) == 1;
            if (tutorialCompleted && disableAfterCompletion)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        popupPanel.SetActive(false);
        
        if (finishIndicator != null)
        {
            finishIndicator.SetActive(false);
        }

        foreach (var msg in tutorialMessages)
        {
            if (msg.associatedObject != null) 
                msg.associatedObject.SetActive(false);
        }

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(ShowNextMessage);
        
        if (continueButtonText != null)
        {
            continueButtonText.text = continueButtonTextDefault;
        }
    }

    private void ForceCloseTutorial()
    {
        // Close the tutorial and clean up regardless of completion state
        popupPanel.SetActive(false);
        isPlayerInTrigger = false;
        
        if (currentActiveGameObject != null)
        {
            currentActiveGameObject.SetActive(false);
            currentActiveGameObject = null;
        }

        // Reset message index but don't affect completion state
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
    }

    private void Update()
    {
        if (isPlayerInTrigger && allowKeyboardInput && Input.GetKeyDown(continueKey))
        {
            ShowNextMessage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !tutorialCompleted)
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
            
            if (continueButtonText != null)
            {
                continueButtonText.text = (currentMessageIndex == tutorialMessages.Count - 1) 
                    ? continueButtonTextFinal 
                    : continueButtonTextDefault;
            }

            if (messagesRemainingForCurrentObject <= 0 && currentMessage.associatedObject != null)
            {
                if (currentActiveGameObject != null)
                {
                    currentActiveGameObject.SetActive(false);
                }
                
                currentActiveGameObject = currentMessage.associatedObject;
                currentActiveGameObject.SetActive(true);
                messagesRemainingForCurrentObject = currentMessage.objectActiveForMessages;
            }
            
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
            CompleteTutorial();
        }
    }

    private void CompleteTutorial()
    {
        tutorialCompleted = true;
        
        if (persistCompletionState)
        {
            PlayerPrefs.SetInt("TutorialCompleted_" + tutorialID, 1);
            PlayerPrefs.Save();
        }

        if (disableAfterCompletion)
        {
            popupPanel.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            if (finishIndicator != null)
            {
                finishIndicator.SetActive(true);
            }
            continueButton.gameObject.SetActive(false);
        }
        
        if (currentActiveGameObject != null)
        {
            currentActiveGameObject.SetActive(false);
            currentActiveGameObject = null;
        }
    }

    public void ResetTutorial()
    {
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
        tutorialCompleted = false;
        continueButton.gameObject.SetActive(true);
        
        if (persistCompletionState)
        {
            PlayerPrefs.SetInt("TutorialCompleted_" + tutorialID, 0);
            PlayerPrefs.Save();
        }
        
        if (finishIndicator != null)
        {
            finishIndicator.SetActive(false);
        }
        
        if (continueButtonText != null)
        {
            continueButtonText.text = continueButtonTextDefault;
        }

        if (currentActiveGameObject != null)
        {
            currentActiveGameObject.SetActive(false);
            currentActiveGameObject = null;
        }
    }

    public void StartTutorial()
    {
        if (tutorialCompleted) return;
        
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
        ShowCurrentMessage();
        popupPanel.SetActive(true);
        isPlayerInTrigger = true;
    }
}
