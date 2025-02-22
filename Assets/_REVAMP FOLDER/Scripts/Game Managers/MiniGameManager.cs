using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;
    public ObstacleSpawner obstacleSpawner;

    [SerializeField] Image backgroundPanel;
    [SerializeField] GameObject miniGameParent;
    [SerializeField] GameObject endMiniGameCanvas;
    [SerializeField] GameObject startGameButton;
    [SerializeField] TextMeshProUGUI endMiniGameText;

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
    }

    public void OpenMiniGameUI()
    {
        isMiniGameActive = false;
        StartCoroutine(FadeBackground(0, 1, () => {
            miniGameParent.SetActive(true);
            startGameButton.SetActive(true);
        }));
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        PlayerMove.isUIOpen = true;

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

    //Accessed by the Button UI
    public void CloseMiniGame()
    {
        // Hide the minigame UI
        miniGameParent.SetActive(false);
        PlayerMove.isUIOpen = false;
        StartCoroutine(FadeBackground(1, 0, null));

        // Destroy any existing obstacles and reset the spawner
        if (obstacleSpawner != null)
        {
            Debug.Log("ObstacleSpawner found. Resetting.");
            obstacleSpawner.ResetSpawner();
        }
        else
        {
            Debug.LogError("ObstacleSpawner not found!");
        }

        // Get the current game timestamp
        GameTimeStamp currentTime = TimeManager.Instance.GetGameTimeStamp();

        // Create a new timestamp for 5 PM the same or next day
        GameTimeStamp timestampOfNextDay = new GameTimeStamp(currentTime);
        timestampOfNextDay.hour = 17; // 5 PM
        timestampOfNextDay.minute = 0;

        // Update the time in the TimeManager
        TimeManager.Instance.SkipTime(timestampOfNextDay);
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
