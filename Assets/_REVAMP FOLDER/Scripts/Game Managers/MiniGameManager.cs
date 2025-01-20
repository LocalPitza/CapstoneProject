using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;
    public ObstacleSpawner obstacleSpawner;

    [SerializeField] GameObject miniGameParent;
    [SerializeField] GameObject endMiniGameCanvas;
    [SerializeField] TextMeshProUGUI endMiniGameText;

    private bool isMiniGameActive = true;

    private void Awake()
    {
        if(instance == null)
        {
           instance = this;
        }

        endMiniGameCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!isMiniGameActive)
        {
            return; // Skip updates if the minigame is inactive
        }
    }

    public void StartMiniGame()
    {
        isMiniGameActive = true;
        miniGameParent.SetActive(true);

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
        // Hide the minigame UI
        miniGameParent.SetActive(false);

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
}
