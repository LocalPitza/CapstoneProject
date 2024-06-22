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
        OnDayAdvanced?.Invoke();
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
