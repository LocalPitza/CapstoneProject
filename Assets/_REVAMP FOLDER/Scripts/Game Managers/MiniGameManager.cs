using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;

    [SerializeField] GameObject gameOverCanvas;

    private bool isMiniGameActive = true;

    private void Awake()
    {
        if(instance == null)
        {
           instance = this;
        }

        isMiniGameActive = true;
        gameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!isMiniGameActive)
        {
            return; // Skip updates if the minigame is inactive
        }
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        isMiniGameActive = false;

        // Get the current score and add it to the player's money
        int finalScore = MiniGameScore.instance.GetScore();
        PlayerStats.Earn(finalScore);

        Debug.Log($"Game Over! Score: {finalScore} added to player's money. Total Money: {PlayerStats.Money}");
    }

    public bool IsMiniGameActive()
    {
        return isMiniGameActive;
    }
}
