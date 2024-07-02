using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DayCounter : MonoBehaviour
{
    public static DayCounter Instance { get; private set; }
    public event Action OnDayAdvanced;

    public TextMeshProUGUI dayText;
    private int currentDay = 1;
    private int currentWeek = 1;

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
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }
        UpdateDayText();
        OnDayAdvanced?.Invoke();
    }

    private void UpdateDayText()
    {
        if (dayText != null)
        {
            dayText.text = "Week: " + currentWeek + "| Day: " + currentDay;
        }
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    public int GetCurrentWeek()
    {
        return currentWeek;
    }
}
