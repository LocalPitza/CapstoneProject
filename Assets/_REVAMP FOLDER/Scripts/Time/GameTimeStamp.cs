using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStamp
{
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

    public int year;
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

    public GameTimeStamp(GameTimeStamp timeStamp)
    {
        this.year = timeStamp.year;
        this.season = timeStamp.season;
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
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
        if (day > 30)
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

    public static int TimestampInMinutes(GameTimeStamp timeStamp)
    {
        return HoursToMinutes(DaysToHours(YearsToDays(timeStamp.year))
            + DaysToHours(SeasonToDays(timeStamp.season))
            + DaysToHours(timeStamp.day)
            + timeStamp.hour) 
            + timeStamp.minute;
    }

    //Calculate the different between 2 timestamps in hours
    public static int CompareTimestamp(GameTimeStamp timeStamp1, GameTimeStamp timeStamp2)
    {
        //Convert timestamps to hours
        int timestamp1Hours = DaysToHours(YearsToDays(timeStamp1.year))
            + DaysToHours(SeasonToDays(timeStamp1.season))
            + DaysToHours(timeStamp1.day)
            + timeStamp1.hour;

        int timestamp2Hours = DaysToHours(YearsToDays(timeStamp2.year))
            + DaysToHours(SeasonToDays(timeStamp2.season))
            + DaysToHours(timeStamp2.day)
            + timeStamp2.hour;

        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);
    }
}
