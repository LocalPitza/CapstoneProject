using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameScore : MonoBehaviour
{
    public static MiniGameScore instance;

    [SerializeField] TextMeshProUGUI scoreText;

    int score;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score; // Return the current score
    }

    public void ResetScore()
    {
        score = 0; // Assuming 'score' is the variable storing the player's score
        scoreText.text = score.ToString();
    }
}
