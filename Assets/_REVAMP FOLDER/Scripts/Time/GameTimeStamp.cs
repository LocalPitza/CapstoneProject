using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp
{
    public int year;
    public enum Season
    {
        Rainy,
        CoolDry,
        HotDry
    }
    public Season season;

    public enum DayOfTheWeek
    {
        SAT,
        SUN,
        MON,
        TUE,
        WED,
        THU,
        FRI
    }

    public int day;
    public int hour;
    public int minute;

    public GameTimeStamp(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }

    public void UpdateClock()
    {
        minute++;

        //60 Minutes in 1 hour
        if(minute >= 60)
        {
            minute = 0;
            hour++;
        }

        //24 hours in 1 day
        if(hour >= 24)
        {
            hour = 0;
            day++;
        }

        //Rainy 152 Days and Dry 213
        if (day > 10)
        {
            day = 1;

            if(season == Season.HotDry)
            {
                season = Season.Rainy;
                year++;
            }
            else
            {
                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        //Convert the totall time passed into days
        int daysPassed = YearsToDays(year) + SeasonToDays(season) + day;

        //Remainder after dividing daysPassed by 7
        int dayIndex = daysPassed % 7;

        //Cast into Day of the Week
        return (DayOfTheWeek)dayIndex;
    }

    //Convert hours to minutes
    public static int HoursToMinutes(int hours)
    {
        return hours * 60;
    }

    //Convert Days to Hours
    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    //Convert Seasons to Days
    public static int SeasonToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    //Years to Days
    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }
}
