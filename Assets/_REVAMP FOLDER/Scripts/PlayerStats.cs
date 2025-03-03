using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int Money {  get; private set; }
    public static int Stamina;
    public static int Hunger;

    public static bool hasPlayedMiniGame = false;
    public static bool isStaminaDepleted = false; // Track if stamina was depleted
    public static bool isHungerDepleted = false;  // Track if hunger was depleted

    public const string CURRENCY = "G";
    public const int STARTING_MONEY = 2000;

    public const int STARTING_HUNGER = 100;

    static PlayerStats()
    {
        Money = STARTING_MONEY;
        Hunger = STARTING_HUNGER;
        NewUIManager.Instance?.RenderPlayerStats();
    }

    public static void Spend(int cost, string cause = "")
    {
        if (cost > Money)
        {
            Debug.LogWarning("Player does not have enough money");
            string gameOverCause = string.IsNullOrEmpty(cause) ? "Failed to pay an expense" : cause;
            GameOver.Instance.GameIsOver(gameOverCause);
            return;
        }
        Money -= cost;

        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void Earn(int income)
    {
        Money += income;

        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void LoadStats(int money, int stamina, int hunger)
    {
        Money = money;
        Stamina = stamina;
        Hunger = hunger;
        RestoreStamina(stamina);
        HungerStat(hunger);
        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void UseStamina(int staminaLost)
    {
        /*Stamina -= staminaLost;
        NewUIManager.Instance.RenderPlayerStats();
        CheckHungerEnergy();*/

        // If stamina is already zero and they attempt another action, trigger medical expense
        if (Stamina <= 0)
        {
            if (!isStaminaDepleted)
            {
                isStaminaDepleted = true; // Mark that stamina was depleted
            }
            else
            {
                Debug.Log("Game Over: Player performed action with zero stamina.");
                MedicalPay.Instance.PayMedical("Stamina");

                // If already in Bedroom, restore Stamina immediately
                if (SceneTransitionManager.Instance.currentLocation == SceneTransitionManager.Location.Bedroom)
                {
                    Stamina = 50;
                    isStaminaDepleted = false;
                    NewUIManager.Instance.RenderPlayerStats();
                }
                else
                {
                    SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.Bedroom);
                    Stamina = 50;
                    isStaminaDepleted = false;
                    NewUIManager.Instance.RenderPlayerStats();
                }
            }
            return;
        }

        // Normal stamina usage
        Stamina -= staminaLost;
        NewUIManager.Instance.RenderPlayerStats();

        // Check if stamina just dropped to zero
        if (Stamina <= 0)
        {
            isStaminaDepleted = true; // Set flag when stamina reaches zero
        }
    }

    public static void RestoreStamina(int amount)
    {
        Stamina += amount;

        if(Stamina > 100)
        {
            Stamina = 100;
        }
        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void HungerStat(int restore)
    {
        /*Hunger += restore;
        NewUIManager.Instance.RenderPlayerStats();
        CheckHungerEnergy();*/

        // If hunger is already zero and player attempts another action, trigger medical expense
        if (Hunger <= 0)
        {
            if (!isHungerDepleted)
            {
                isHungerDepleted = true; // Mark that hunger was depleted
            }
            else
            {
                Debug.Log("Game Over: Zero hunger for a set of time.");
                MedicalPay.Instance.PayMedical("Hunger");
                Hunger = 50;
                isHungerDepleted = false;

                // If already in Bedroom, restore Hunger immediately
                if (SceneTransitionManager.Instance.currentLocation == SceneTransitionManager.Location.NewBedroom)
                {
                    Hunger = 50;
                    isHungerDepleted = false;
                    NewUIManager.Instance.RenderPlayerStats();
                }
                else
                {
                    SceneTransitionManager.Instance.SwitchLocation(SceneTransitionManager.Location.NewBedroom);
                    Hunger = 50;
                    isHungerDepleted = false;
                    NewUIManager.Instance.RenderPlayerStats();
                }
            }
            return;
        }

        // Normal hunger increase
        Hunger += restore;
        NewUIManager.Instance.RenderPlayerStats();

        // Check if hunger just dropped to zero
        if (Hunger <= 0)
        {
            isHungerDepleted = true; // Set flag when hunger reaches zero
        }
    }

    // Method to check if the player has already played the mini-game
    public static bool HasPlayedMiniGame()
    {
        return hasPlayedMiniGame;
    }

    // Method to mark the mini-game as played
    public static void MarkMiniGameAsPlayed()
    {
        hasPlayedMiniGame = true;
    }

    // Method to reset the mini-game status (useful if you want to allow playing again)
    public static void ResetMiniGameStatus()
    {
        hasPlayedMiniGame = false;
    }
}
