using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    
    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject backdropPanel;

    [SerializeField] private LoadingManager loadingManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenuUI.activeSelf)
            {
                // Close settings and return to the pause menu
                OpenPauseMenu();
            }
            else
            {
                TogglePause();
            }
        }
    }

    private void TogglePause()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        backdropPanel.SetActive(false);
        Time.timeScale = 1f;
        CursorManager.Instance?.UIClosed();
        GameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
        backdropPanel.SetActive(true);
        CursorManager.Instance?.UIOpened();
        GameIsPaused = true;
    }

    public void OpenSettings()
    {
        settingsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void LoadMenu()
    {
        if (loadingManager != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            loadingManager.LoadScene("NewMenu");
        }
        else
        {
            Debug.LogWarning("LoadingManager is not assigned in PauseMenu!");
        }
    }
}
