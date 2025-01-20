using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorkTimer : MonoBehaviour
{
    public static WorkTimer instance;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float initialTime = 10f;
    private float countDown;

    public bool isCountingDown = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartTimer(initialTime);
    }

    private void Update()
    {
        if (isCountingDown)
        {
            countDown -= Time.deltaTime; // Reduce the timer by the elapsed time
            UpdateTimerText();

            if (countDown <= 0)
            {
                countDown = 0; // Clamp to zero
                isCountingDown = false;
                EndMinigame(); // Call a method to end the game
            }
        }
    }

    public void StartTimer(float duration)
    {
        countDown = duration;
        isCountingDown = true;
    }

    public void ResetTimer()
    {
        countDown = initialTime;
        UpdateTimerText();
        isCountingDown = false;
    }

    public void StopTimer()
    {
        isCountingDown = false; // Stop the countdown
    }

    private void UpdateTimerText()
    {
        // Display the timer in MM:SS format
        int seconds = Mathf.FloorToInt(countDown);
        timerText.text = seconds.ToString("00");
    }

    private void EndMinigame()
    {
        // Notify the minigame manager to end the game
        MiniGameManager minigameManager = FindObjectOfType<MiniGameManager>();
        if (minigameManager != null)
        {
            MiniGameManager.instance.GameOver();
        }
    }
}
