using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentRent : MonoBehaviour, ITimeTracker
{
    [SerializeField] private int rentMin = 500; // Minimum rent
    [SerializeField] private int rentMax = 1000; // Maximum rent
    [Range(15,30)]
    [SerializeField] private int dayOfRent = 30;
    private int lastProcessedDay = 0;

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

    public void ClockUpdate(GameTimeStamp gameTimeStamp)
    {
        // Check if the day has reached 30 and hasn't been processed for this month
        if (gameTimeStamp.day == dayOfRent && lastProcessedDay != dayOfRent)
        {
            DeductRent();
            lastProcessedDay = dayOfRent; // Mark the day as processed
        }
        else if (gameTimeStamp.day != dayOfRent)
        {
            // Reset the tracker when it's no longer day 30
            lastProcessedDay = gameTimeStamp.day;
        }
    }

    void DeductRent()
    {
        // Generate a random rent amount within the specified range
        int rentAmount = Random.Range(rentMin, rentMax + 1);

        // Deduct the rent amount from the player's money
        PlayerStats.Spend(rentAmount);

        // Log the rent for debugging purposes
        Debug.LogWarning($"Rent day! {rentAmount}{PlayerStats.CURRENCY} has been deducted for rent. Remaining Money: {PlayerStats.Money}{PlayerStats.CURRENCY}");
    }
}
