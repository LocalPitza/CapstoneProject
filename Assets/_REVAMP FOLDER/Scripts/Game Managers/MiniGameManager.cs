using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;
    public ObstacleSpawner obstacleSpawner;

    [SerializeField] Image backgroundPanel;
    [SerializeField] GameObject miniGameParent;
    [SerializeField] GameObject endMiniGameCanvas;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TextMeshProUGUI endMiniGameText;

    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage videoScreen;
    [SerializeField] private VideoClip goingToWorkVideo;
    [SerializeField] private VideoClip goingHomeVideo;

    private bool isMiniGameActive = true;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if(instance == null)
        {
           instance = this;
        }

        endMiniGameCanvas.SetActive(false);
        miniGameParent.SetActive(false);
        backgroundPanel.color = new Color(backgroundPanel.color.r, backgroundPanel.color.g, backgroundPanel.color.b, 0);
        videoScreen.gameObject.SetActive(false);
    }

    public void OpenMiniGameUI()
    {
        isMiniGameActive = false;
        PlayerMove.isUIOpen = true;
        StartCoroutine(PlayVideoBeforeMinigame());
    }

    private IEnumerator PlayVideoBeforeMinigame()
    {
        yield return StartCoroutine(FadeBackground(0, 1, null));

        videoScreen.gameObject.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.clip = goingToWorkVideo;
        videoPlayer.time = 0;
        videoPlayer.Play();

        Debug.Log("Playing GoingToWork video...");

        // Wait for the video to start playing
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Video started playing");

        // Wait for the video to finish
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Video finished playing, hiding screen and showing minigame UI");

        videoScreen.gameObject.SetActive(false);

        // Delay before showing minigame UI
        yield return new WaitForSeconds(0.5f);

        miniGameParent.SetActive(true);
        startGameButton.SetActive(true);
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;

        startGameButton.SetActive(false);

        // Reset the timer
        WorkTimer.instance.ResetTimer();

        // Reset player's position and gravity
        FlyBehavior flyBehavior = FindObjectOfType<FlyBehavior>();
        if (flyBehavior != null)
        {
            flyBehavior.ResetPosition(); // Reset to a default position
            flyBehavior.ResetGravity(); // Reset gravity to its initial state
        }

        // Reset the score
        MiniGameScore.instance.ResetScore();

        // Hide the end game canvas
        endMiniGameCanvas.SetActive(false);
    }

    public void GameOver()
    {
        isMiniGameActive = false;

        WorkTimer.instance.StopTimer();

        FlyBehavior flyBehavior = FindObjectOfType<FlyBehavior>();
        if (flyBehavior != null)
        {
            flyBehavior.DisableGravity();
        }


        // Get the current score and add it to the player's money
        int finalScore = MiniGameScore.instance.GetScore();
        PlayerStats.Earn(finalScore);

        TriggerEndMiniGame($"{finalScore} added to player's money");

        Debug.Log($"Score: {finalScore} added to player's money. Total Money: {PlayerStats.Money}");
    }

    public bool IsMiniGameActive()
    {
        return isMiniGameActive;
    }

    public void TriggerEndMiniGame(string message)
    {
        endMiniGameCanvas.SetActive(true);
        endMiniGameText.text = message;
    }

    public void CloseMiniGame()
    {
        miniGameParent.SetActive(false);
        PlayerMove.isUIOpen = false;
        StartCoroutine(PlayClosingVideo());
    }

    private IEnumerator PlayClosingVideo()
    {
        videoScreen.gameObject.SetActive(true);
        videoPlayer.Stop();
        videoPlayer.clip = goingHomeVideo;
        videoPlayer.time = 0;
        videoPlayer.Play();

        Debug.Log("Playing GoingHome video...");

        // Wait for the video to start playing
        while (!videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Video started playing");

        // Wait for the video to finish
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Video finished playing, skipping time before fade-out");

        // Skip time immediately after the video finishes
        GameTimeStamp currentTime = TimeManager.Instance.GetGameTimeStamp();
        GameTimeStamp timestampOfNextDay = new GameTimeStamp(currentTime);
        timestampOfNextDay.hour = 17;
        timestampOfNextDay.minute = 0;
        TimeManager.Instance.SkipTime(timestampOfNextDay);

        // Hide the video screen
        videoScreen.gameObject.SetActive(false);

        // Now proceed with the fade-out
        StartCoroutine(FadeBackground(1, 0, () => {
            if (obstacleSpawner != null)
            {
                Debug.Log("ObstacleSpawner found. Resetting.");
                obstacleSpawner.ResetSpawner();
            }
            else
            {
                Debug.LogError("ObstacleSpawner not found!");
            }
        }));
    }

    private IEnumerator FadeBackground(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float elapsedTime = 0f;
        Color color = backgroundPanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            backgroundPanel.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        backgroundPanel.color = new Color(color.r, color.g, color.b, endAlpha);
        onComplete?.Invoke();
    }
}
