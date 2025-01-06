using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    public static int Money {  get; private set; }
    public static int Stamina;

    public const string CURRENCY = "G";
    public const int STARTING_MONEY = 2000;

    static PlayerStats()
    {
        Money = STARTING_MONEY;
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

    public static void LoadStats(int money, int stamina)
    {
        Money = money;
        Stamina = stamina;
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
}
