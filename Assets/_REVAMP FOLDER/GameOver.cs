using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    [SerializeField] GameObject gameOverScreenUI;
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

    public void GameIsOver()
    {
        medicalUI.SetActive(false);
        gameOverScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }


}
