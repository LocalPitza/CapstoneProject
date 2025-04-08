using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OpeningCutscene : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] Button startGameButton;

    private void Start()
    {
        // Enable the start button at the start
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        // Disable the start button so it doesn't obstruct the loading screen
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(false);

        // Load the next scene using the loading manager
        if (loadingManager != null)
        {
            string bedroom = SceneTransitionManager.Location.NewBedroom.ToString();
            loadingManager.LoadScene(bedroom);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }



    //[SerializeField] private VideoPlayer videoPlayer;

    /*private void Start()
    {
        // Disable the button at the start
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(false);

        // Reset video to start from the beginning
        if (videoPlayer != null)
        {
            videoPlayer.frame = 0;
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Enable the start button when the video ends
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        // Disable the video player (or hide its GameObject)
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
        }

        // Disable the start button so it doesn't obstruct the loading screen
        if (startGameButton != null)
            startGameButton.gameObject.SetActive(false);

        // Load the next scene using the loading manager
        if (loadingManager != null)
        {
            string bedroom = SceneTransitionManager.Location.NewBedroom.ToString();
            loadingManager.LoadScene(bedroom);
        }
    }*/
}
