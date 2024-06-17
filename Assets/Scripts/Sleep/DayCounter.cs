using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayCounter : MonoBehaviour
{
    public static DayCounter Instance { get; private set; }

    public TextMeshProUGUI dayText;
    private int currentDay = 1;

    void Awake()
    {
        // Ensure only one instance of DayCounter exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateDayText();
    }

    public void AdvanceDay()
    {
        currentDay++;
        UpdateDayText();
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = "Day: " + currentDay;
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }
}
