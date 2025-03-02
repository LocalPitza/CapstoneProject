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
    public TMP_Text descriptionText;
    
    [System.Serializable]
    public class JournalEntry
    {
        public string name;
        public ItemData itemData; // Use Scriptable Object for description
        public string customDescription; 
        public Button buttonPrefab;
    }
    
    public List<JournalEntry> seedsList;
    public List<JournalEntry> guideList;
    public List<JournalEntry> itemsList;
    
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleJournal();
        }
    }

    void ToggleJournal()
    {
        //journalPanel.SetActive(!journalPanel.activeSelf);

        bool isActive = !journalPanel.activeSelf;
        journalPanel.SetActive(isActive);

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void ShowMenu(GameObject menu, List<JournalEntry> entries, bool isGuide)
    {
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
            newButton.onClick.AddListener(() => ShowDescription(entry.itemData != null ? entry.itemData.description : "No description available"));
        }
    }
    
    void ShowDescription(string text)
    {
        descriptionText.text = text;
    }
}
