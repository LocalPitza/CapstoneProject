using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    [SerializeField] LoadingManager loadingManager;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        CursorManager.Instance.UIClosed();
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        CursorManager.Instance.UIOpened();
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        // Load the Main Menu
        if (loadingManager != null)
        {
            loadingManager.LoadScene("NewMenu");
            CursorManager.Instance.UIClosed();
        }
    }

}
