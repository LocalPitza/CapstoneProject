using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    // Key bindings (defaults)
    public KeyCode interactKey;
    public KeyCode harvestKey;
    //public KeyCode harvestKeepKey = KeyCode.Q;
    public KeyCode openJournal;
    public KeyCode openBackpack;

    private void Awake()
    {
        // Ensure a single instance of InputManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        LoadKeyBindings();
    }
        public void LoadKeyBindings()
    {
        interactKey = (KeyCode) PlayerPrefs.GetInt("InteractKey", (int)KeyCode.F);
        harvestKey = (KeyCode) PlayerPrefs.GetInt("HarvestKey", (int)KeyCode.E);
        openJournal = (KeyCode) PlayerPrefs.GetInt("JournalKey", (int)KeyCode.O);
        openBackpack = (KeyCode) PlayerPrefs.GetInt("BackpackKey", (int)KeyCode.I);
    }

    public void SetKeyBinding(string keyName, KeyCode newKey)
    {
        switch (keyName)
        {
            case "Interact":
                interactKey = newKey;
                PlayerPrefs.SetInt("InteractKey", (int)newKey);
                break;
            case "Harvest":
                harvestKey = newKey;
                PlayerPrefs.SetInt("HarvestKey", (int)newKey);
                break;
            case "Journal":
                openJournal = newKey;
                PlayerPrefs.SetInt("JournalKey", (int)newKey);
                break;
            case "Backpack":
                openBackpack = newKey;
                PlayerPrefs.SetInt("BackpackKey", (int)newKey);
                break;
        }
        PlayerPrefs.Save();
    }
}
