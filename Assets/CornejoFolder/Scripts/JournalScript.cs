using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class JournalScript : MonoBehaviour
{
    public GameObject journalPanel;
    public Button openJournalButton;
    public Button seedsButton, guideButton, itemsButton;
    public GameObject seedsMenu, guideMenu, itemsMenu;

    public Image itemIcon;
    public TMP_Text descriptionText;
    public Transform listingGrid;

    [System.Serializable]
    public class JournalEntry
    {
        public string name;
        public ItemData itemData; // Use Scriptable Object for description
        [TextArea(5,5)]
        public string customDescription; 
        public Button buttonPrefab;
    }
    
    public List<JournalEntry> seedsList;
    public List<JournalEntry> guideList;
    public List<JournalEntry> itemsList;
    
    void Start()
    {
        Debug.Log("IsNewGame: " + PlayerPrefs.GetInt("IsNewGame", 0));

        if (PlayerPrefs.GetInt("IsNewGame", 1) == 1)
        {
            journalPanel.SetActive(true);
            ShowMenu(guideMenu, guideList, false);
            PlayerPrefs.SetInt("IsNewGame", 0); // Mark as no longer a New Game
            PlayerPrefs.Save();
        }
        else
        {
            journalPanel.SetActive(false);
        }

        openJournalButton.onClick.AddListener(ToggleJournal);
        seedsButton.onClick.AddListener(() => ShowMenu(seedsMenu, seedsList, false));
        guideButton.onClick.AddListener(() => ShowMenu(guideMenu, guideList, true));
        itemsButton.onClick.AddListener(() => ShowMenu(itemsMenu, itemsList, false));
        
        journalPanel.SetActive(false);
        seedsMenu.SetActive(false);
        guideMenu.SetActive(false);
        itemsMenu.SetActive(false);
    }

    void Update()
    {
        // Prevent opening if multiple UIs are already open
        if (CursorManager.Instance.GetUIOpenCount() > 1) return;

        if (Input.GetKeyDown(InputManager.Instance.openJournal))
        {
            FindObjectOfType<SoundManager>().Play("SNBClick");
            ToggleJournal();
        }
    }

    void ToggleJournal()
    {
        // If multiple UIs are open, don't allow opening
        if (!journalPanel.activeSelf && CursorManager.Instance.GetUIOpenCount() >= 1) return;

        descriptionText.text = "";

        //Resets the listings if there was a previous one
        if (listingGrid.childCount > 0)
        {
            foreach (Transform child in listingGrid)
            {
                Destroy(child.gameObject);
            }
        }

        bool isActive = !journalPanel.activeSelf;
        journalPanel.SetActive(isActive);

        if (isActive)
        {
            CursorManager.Instance.UIOpened();

            // Show the Guides by default when opening the Journal
            ShowMenu(guideMenu, guideList, false);
        }
        else
        {
            CursorManager.Instance.UIClosed();
        }
    }
    
    void ShowMenu(GameObject menu, List<JournalEntry> entries, bool isGuide)
    {
        // Clear the description text and Icon when changing menus
        descriptionText.text = "";
        itemIcon.gameObject.SetActive(false);

        seedsMenu.SetActive(false);
        guideMenu.SetActive(false);
        itemsMenu.SetActive(false);
        menu.SetActive(true);
        PopulateMenu(menu, entries, isGuide);
    }
    
    void PopulateMenu(GameObject menu, List<JournalEntry> entries, bool isGuide)
    {
        foreach (Transform child in menu.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (JournalEntry entry in entries)
        {
            Button newButton = Instantiate(entry.buttonPrefab, menu.transform);
            newButton.GetComponentInChildren<TMP_Text>().text = entry.name;

            string descriptionToShow = string.IsNullOrEmpty(entry.customDescription) ? entry.itemData.description : entry.customDescription;
            Sprite iconToShow = entry.itemData.thumbnail; // Get the thumbnail

            newButton.onClick.AddListener(() => ShowDescription(descriptionToShow, iconToShow));
        }
    }

    void ShowDescription(string text, Sprite icon)
    {
        descriptionText.text = text;
        itemIcon.sprite = icon;
        itemIcon.gameObject.SetActive(icon != null); // Hide if no icon
    }
}
