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
    [SerializeField] private string uniqueTutorialID = "unique_tutorial_id";

    [Header("Input Settings")]
    [SerializeField] private KeyCode continueKey = KeyCode.Space;
    [SerializeField] private bool allowKeyboardInput = true;

    private int currentMessageIndex = 0;
    private bool isPlayerInTrigger = false;
    private GameObject currentActiveGameObject = null;
    private int messagesRemainingForCurrentObject = 0;
    private static Dictionary<string, bool> tutorialCompletionStates = new Dictionary<string, bool>();

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Initialize completion state if not already set
        if (!tutorialCompletionStates.ContainsKey(uniqueTutorialID))
        {
            tutorialCompletionStates.Add(uniqueTutorialID, false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ForceCloseTutorial();
    }

    private void Start()
    {
        InitializeTutorial();
        
        // Disable immediately if already completed
        if (tutorialCompletionStates[uniqueTutorialID] && disableAfterCompletion)
        {
            gameObject.SetActive(false);
        }
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
        popupPanel.SetActive(false);
        isPlayerInTrigger = false;
        
        if (currentActiveGameObject != null)
        {
            currentActiveGameObject.SetActive(false);
            currentActiveGameObject = null;
        }

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
        if (other.CompareTag("Player") && !tutorialCompletionStates[uniqueTutorialID])
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
        tutorialCompletionStates[uniqueTutorialID] = true;

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
        tutorialCompletionStates[uniqueTutorialID] = false;
        continueButton.gameObject.SetActive(true);
        
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
        if (tutorialCompletionStates[uniqueTutorialID]) return;
        
        currentMessageIndex = 0;
        messagesRemainingForCurrentObject = 0;
        ShowCurrentMessage();
        popupPanel.SetActive(true);
        isPlayerInTrigger = true;
    }
}
