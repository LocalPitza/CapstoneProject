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
            // Reset time and pause state
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CursorManager.Instance?.ResetUICount();

            // Set IsNewGame to ensure the Journal shows up
            PlayerPrefs.SetInt("IsNewGame", 1);
            PlayerPrefs.Save();

            /*string bedroom = SceneTransitionManager.Location.NewBedroom.ToString();
            loadingManager.LoadScene(bedroom);*/

            loadingManager.LoadScene("OpeningCutscene");

            // Clear other data but keep IsNewGame
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("IsNewGame", 1);
            PlayerPrefs.Save();

            PlayerStats.Hunger = 100;
            PlayerStats.Stamina = 100;
            Debug.Log("Starting a New Game. Journal will show.");
        }
    }

    public void ContinueGame()
    {
        if (loadingManager != null)
        {
            // Reset time and pause state
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            CursorManager.Instance?.ResetUICount();

            string bedroom = SceneTransitionManager.Location.NewBedroom.ToString();
            loadingManager.LoadScene(bedroom, LoadGame);

            PlayerPrefs.SetInt("IsNewGame", 0); // Mark it as Continue Game
            PlayerPrefs.Save();
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
