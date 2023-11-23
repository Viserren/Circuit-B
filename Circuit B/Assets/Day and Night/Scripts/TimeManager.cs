using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    [Header("Date & Time Settings")]
    [Range(1, 28)]
    public int dateInMonth;
    [Range(1, 4)]
    public int season;
    public int year;
    [Range(0, 23)]
    public int hours;
    [Range(0, 60)]
    public int minutes;
    [Range(0, 60)]
    public int seconds;

    private DateTime dateTime;

    [Header("Tick Settings")]
    //public int randomTickSpeed = 3;
    public float advanceTimeIncrement = 4.24f;

    [Header("Events")]
    public UnityEvent<DateTime> OnDateChanged;
    // Add evnts here

    private void Awake()
    {
        dateTime = new DateTime(dateInMonth, season - 1, year, hours, minutes, seconds);
    }

    private void Start()
    {
        OnDateChanged.Invoke(dateTime);
        StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            Tick();
        }
    }

    void Tick()
    {
        AdvanceTime();
    }

    void AdvanceTime()
    {

        dateTime.AdvanceSeconds(advanceTimeIncrement);
        OnDateChanged?.Invoke(dateTime);
    }
}

public struct DateTime
{
    #region Fields
    private Days day;
    private int date;
    private int year;

    private int hours;
    private int minutes;
    private float seconds;

    private Seasons season;

    private float totalNumberOfSecondsInDay;
    private int totalNumberOfMinutesInDay;
    private int totalNumberOfdays;
    private int totalNumberOfWeeks;
    #endregion

    #region Properties
    public Days Day => day;
    public int Date => date;
    public int Hours => hours;
    public int Minutes => minutes;
    public float Seconds => seconds;
    public Seasons Season => season;
    public int Year => year;
    public float TotalNumberOfSecondsInDay => totalNumberOfSecondsInDay;
    public int TotalNumberOfMinutesInDay => totalNumberOfMinutesInDay;
    public int TotalNumberOfDays => totalNumberOfdays;
    public int TotalNumberOfWeeks => totalNumberOfWeeks;
    public int CurrentWeek => totalNumberOfWeeks % 16 == 0 ? 16 : totalNumberOfWeeks % 16;
    #endregion

    #region Constructor
    public DateTime(int date, int season, int year, int hours, int minutes, float seconds)
    {
        this.day = (Days)(date % 7);
        if (day == 0) day = (Days)7;
        this.date = date;
        this.season = (Seasons)season;
        this.year = year;

        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;

        totalNumberOfdays = date + (28 * (int)this.season) + (112 * (year - 1));
        totalNumberOfMinutesInDay = (60 * hours) + minutes;
        totalNumberOfSecondsInDay = (totalNumberOfMinutesInDay * 60) + seconds;
        totalNumberOfWeeks = 1 + totalNumberOfdays / 7;
    }
    #endregion

    #region Time Advancement
    public void AdvanceSeconds(float secondsToAdvanceBy)
    {
        seconds += secondsToAdvanceBy;
        totalNumberOfSecondsInDay += secondsToAdvanceBy;
        if (seconds >= 60)
        {
            AdvanceMinutes((int)seconds / 60);
            seconds = seconds % 60;
        }
    }
    private void AdvanceMinutes(int minutesToAdvanceBy)
    {
        if (minutes + minutesToAdvanceBy >= 60)
        {
            minutes = (minutes + minutesToAdvanceBy) % 60;
            AdvanceHour();
        }
        else
        {
            minutes += minutesToAdvanceBy;
            totalNumberOfMinutesInDay += minutesToAdvanceBy;
        }

    }
    void AdvanceHour()
    {
        hours++;
        if ((hours == 24))
        {
            hours = 0;
            AdvanceDay();
        }
    }

    void AdvanceDay()
    {
        day++;
        if (day > (Days)7)
        {
            day = (Days)1;
            totalNumberOfWeeks++;
        }
        date++;
        if (date % 28 == 0)
        {
            AdvanceSeason();
            date = 1;
        }
        totalNumberOfdays++;
        totalNumberOfMinutesInDay = 0;
        totalNumberOfSecondsInDay = 0;
    }

    void AdvanceSeason()
    {
        if (season == Seasons.Winter)
        {
            season = Seasons.Spring;
            AdvanceYear();
        }
        else
        {
            season++;
        }
    }

    void AdvanceYear()
    {
        date = 1;
        year++;
    }
    #endregion

    #region Enums
    [System.Serializable]
    public enum Days
    {
        NULL = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }

    [System.Serializable]
    public enum Seasons
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }
    #endregion

    #region Bool Checks
    public bool IsNight()
    {
        return hours > 13 || hours < 6;
    }

    public bool IsMorning()
    {
        return hours >= 6 && hours < 18;
    }

    public bool IsAfternoon()
    {
        return hours > 12 && hours < 18;
    }

    public bool IsWeekend()
    {
        return day > Days.Friday ? true : false;
    }

    public bool IsParticularDay(Days dayToCheck)
    {
        return day == dayToCheck;
    }
    #endregion

    #region Key Dates
    public DateTime NewYearsDay(int year)
    {
        if (year == 0)
        {
            year = 1;
        }
        return new DateTime(1, 0, year, 6, 0, 0);
    }

    public DateTime SummerSolstice(int year)
    {
        if (year == 0)
        {
            year = 1;
        }
        return new DateTime(21, 1, year, 6, 0, 0);
    }

    public DateTime PumpkinHarvist(int year)
    {
        if (year == 0)
        {
            year = 1;
        }
        return new DateTime(28, 2, year, 6, 0, 0);
    }
    #endregion

    #region Start Of Season
    private DateTime StartOfSeason(int season, int year)
    {
        return new DateTime(1, season, year, 6, 0, 0);
    }
    public DateTime StartOfSpring(int year)
    {
        return StartOfSeason(0, year);
    }
    public DateTime StartOfSummer(int year)
    {
        return StartOfSeason(1, year);
    }
    public DateTime StartOfAutumn(int year)
    {
        return StartOfSeason(2, year);
    }
    public DateTime StartOfWinter(int year)
    {
        return StartOfSeason(3, year);
    }
    #endregion

    #region To String
    public override string ToString()
    {
        return $"Date: {DateToString()} Season: {season.ToString()} Time: {DateToString()} \nTotal Number of Days: {totalNumberOfdays} | Total Weeks: {totalNumberOfWeeks}";
    }

    public string DateToString()
    {
        return $"{day} {date} {year.ToString("D2")}";
    }
    public string TimeToString()
    {
        return $"{hours.ToString("D2")}:{minutes.ToString("D2")}";
    }

    public string TimeAMPMToString()
    {
        int adjustedHour = 0;

        if (hours == 0)
        {
            adjustedHour = 12;
        }
        else if (hours == 24)
        {
            adjustedHour = 12;
        }
        else if (hours >= 13)
        {
            adjustedHour = hours - 12;
        }
        else
        {
            adjustedHour = hours;
        }

        string amPm = hours == 0 || hours < 12 ? "AM" : "PM";

        return $"{adjustedHour.ToString("D2")}:{minutes.ToString("D2")} {amPm}";
    }
    #endregion
}

