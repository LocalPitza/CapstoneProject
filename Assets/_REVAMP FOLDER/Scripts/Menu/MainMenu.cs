using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] Button looadGameButton;

    public void NewGame() 
    {
        if (loadingManager != null)
        {
            string bedroom = SceneTransitionManager.Location.Bedroom.ToString();
            loadingManager.LoadScene(bedroom);

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            PlayerStats.Hunger = 100;
            PlayerStats.Stamina = 100;
            Debug.Log("All PlayerPrefs have been cleared.");
        }
    }

    public void ContinueGame()
    {
        if (loadingManager != null)
        {
            string bedroom = SceneTransitionManager.Location.Bedroom.ToString();
            loadingManager.LoadScene(bedroom, LoadGame);

        }
    }

    void LoadGame()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("No GameStateManager found!");
            return;
        }

        GameStateManager.Instance.LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        looadGameButton.interactable = SaveManager.HasSave();
    }
}
