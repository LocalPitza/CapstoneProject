using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassiveIncome : MonoBehaviour, ITimeTracker
{
    [SerializeField] int passiveIncomeMin = 50;
    [SerializeField] int passiveIncomeMax = 200;
    [Tooltip("The day of the week when passive income is received.")]
    [SerializeField] private GameTimeStamp.DayOfTheWeek incomeDay = GameTimeStamp.DayOfTheWeek.WED;

    public GameObject incomeUI;
    public TextMeshProUGUI incomeText;

    private GameTimeStamp.DayOfTheWeek lastProcessedDay = GameTimeStamp.DayOfTheWeek.SUN;

    void Start()
    {
        // Register this script as a time tracker
        TimeManager.Instance.RegisterTracker(this);
    }

    void OnDestroy()
    {
        // Unregister this script as a time tracker to avoid errors
        TimeManager.Instance.UnregisterTracker(this);
    }

    // Update is called once per frame
    public void ClockUpdate(GameTimeStamp gameTimeStamp)
    {
        // Check if it's the selected income day and ensure it hasn't been processed this week
        if (gameTimeStamp.GetDayOfTheWeek() == incomeDay
            && lastProcessedDay != incomeDay)
        {
            GivePassiveIncome();
            lastProcessedDay = incomeDay;
        }
        else if (gameTimeStamp.GetDayOfTheWeek() != incomeDay)
        {
            // Reset when it's no longer the income day
            lastProcessedDay = gameTimeStamp.GetDayOfTheWeek();
        }
    }

    void GivePassiveIncome()
    {
        // Generate a random amount of money between 50 and 200
        int income = Random.Range(passiveIncomeMin, passiveIncomeMax);

        // Add money to the player's stats
        PlayerStats.Earn(income);

        incomeUI.SetActive(true);
        incomeText.text = "Income this week: " + income;

        CursorManager.Instance.UIOpened();

        // Log the money for debugging purposes
        Debug.Log($"It's Wednesday! You received {income}{PlayerStats.CURRENCY}. Total Money: {PlayerStats.Money}{PlayerStats.CURRENCY}");
    }

    public void CloseIncomeUI()
    {
        incomeUI.SetActive(false);

        CursorManager.Instance.UIClosed();
    }
}
