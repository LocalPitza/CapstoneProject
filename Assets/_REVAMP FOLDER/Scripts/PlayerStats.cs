using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int Money {  get; private set; }
    public static int Stamina;
    public static int Hunger;

    public static bool hasPlayedMiniGame = false;

    public const string CURRENCY = "G";
    public const int STARTING_MONEY = 2000;

    public const int STARTING_HUNGER = 100;

    static PlayerStats()
    {
        Money = STARTING_MONEY;
        Hunger = STARTING_HUNGER;
        NewUIManager.Instance?.RenderPlayerStats();
    }

    public static void Spend(int cost)
    {
        if(cost > Money)
        {
            Debug.LogError("Player does not have enough money");
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
        RestoreStamina();
        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void UseStamina(int staminaLost)
    {
        Stamina -= staminaLost;
        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void RestoreStamina()
    {
        Stamina = 100;
        NewUIManager.Instance.RenderPlayerStats();
    }

    public static void HungerStat(int restore)
    {
        Hunger += restore;
        NewUIManager.Instance.RenderPlayerStats();
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
