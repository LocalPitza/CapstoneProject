using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    [SerializeField] GameObject gameOverScreenUI;
    [SerializeField] TextMeshProUGUI gameOverCauseText;
    [SerializeField] Image gameOverCauseImage;
    [SerializeField] Sprite medicalExpenseSprite;
    [SerializeField] Sprite rentExpenseSprite;
    [SerializeField] GameObject medicalUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameOverScreenUI.SetActive(false);

    }

    public void GameIsOver(string cause)
    {
        medicalUI.SetActive(false);
        gameOverScreenUI.SetActive(true);

        // Set the appropriate image based on the cause
        if (cause == "Failed to pay medical expenses")
        {
            gameOverCauseImage.sprite = medicalExpenseSprite;
        }
        else if (cause == "Failed to pay rent")
        {
            gameOverCauseImage.sprite = rentExpenseSprite;
        }

        gameOverCauseText.text = $"Game Over: {cause}";

        Time.timeScale = 0f;
    }


}
