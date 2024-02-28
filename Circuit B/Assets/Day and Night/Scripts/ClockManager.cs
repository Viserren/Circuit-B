using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    //public RectTransform ClockFace;
    public TextMeshProUGUI Date, Time, Season, Week;

    [SerializeField] AudioMixerSnapshot _daySnapshot,_nightSnapshot;

    //public Image weatherSprite;
    //public Sprite[] weatherSprites;

    private float sunStartingRotation;

    public Light sunLight;
    public float dayIntensity;
    public float nightIntensity;
    //public TimeToUseWhenUpdating timeToUseWhenUpdating;
    public AnimationCurve dayNightCurve;
    public AnimationCurve sunHeightCurve;

    //float tSeconds;
    //float tMinutes;
    //float tHours;

    private void Awake()
    {
        //startingRotation = ClockFace.eulerAngles.z;
        sunStartingRotation = sunLight.transform.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        UpdateRealDateTime();
    }

    public void UpdateRealDateTime()
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;


        int currenWeek = cal.GetWeekOfYear(System.DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        float pos = (float)currenWeek / 52f;
        int totalHours = System.DateTime.Now.Hour;
        int totalMinutes = (System.DateTime.Now.Hour * 60) + System.DateTime.Now.Minute;
        int totalSeconds = (totalMinutes * 60) + System.DateTime.Now.Second;

        float linierTime = (float)totalSeconds / 86400f;
        //Debug.Log($"0 To 1 Time: {linierTime}, 0 To 1 Week: {pos} Seconds: {totalSeconds}");
        float newRotation = Mathf.Lerp(-180, 180, linierTime);

        if(linierTime > .25f && linierTime < .75f)
        {
            _daySnapshot.TransitionTo(20);
        }
        else
        {
            _nightSnapshot.TransitionTo(20);
        }

        // Winter 20
        // Autumn 40
        // Spring 60
        // Summer 80

        Quaternion lowAngle = Quaternion.Euler(20, 0, 0) * Quaternion.Euler(0, newRotation + sunStartingRotation, 0);
        Quaternion highAngle = Quaternion.Euler(80, 0, 0) * Quaternion.Euler(0, newRotation + sunStartingRotation, 0);

        float sunPos = sunHeightCurve.Evaluate(pos);
        float sunIntensity = dayNightCurve.Evaluate(linierTime);
        if (sunLight)
        {
            sunLight.transform.rotation = Quaternion.Lerp(lowAngle, highAngle, sunPos);
            sunLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, sunIntensity);
        }
    }

    //public void UpdateDateTime(DateTime dateTime)
    //{
    //    float pos = (float)dateTime.CurrentWeek / 16;
    //    float newRotation = Mathf.Lerp(-180, 180, t(dateTime));

    //    // Winter 20
    //    // Autumn 40
    //    // Spring 60
    //    // Summer 80

    //    Quaternion lowAngle = Quaternion.Euler(20, 0, 0) * Quaternion.Euler(0, newRotation + sunStartingRotation, 0);
    //    Quaternion highAngle = Quaternion.Euler(80, 0, 0) * Quaternion.Euler(0, newRotation + sunStartingRotation, 0);

    //    float sunPos = sunHeightCurve.Evaluate(pos);
    //    float sunIntensity = dayNightCurve.Evaluate(t(dateTime));
    //    if (sunLight)
    //    {
    //        sunLight.transform.rotation = Quaternion.Lerp(lowAngle, highAngle, sunPos);
    //        sunLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, sunIntensity);
    //    }
    //}

    //float t(DateTime dateTime)
    //{
    //    switch (timeToUseWhenUpdating)
    //    {
    //        case TimeToUseWhenUpdating.seconds:
    //            tSeconds = (float)dateTime.TotalNumberOfSecondsInDay / 86400f;
    //            //Debug.Log(tSeconds);
    //            return tSeconds;
    //        case TimeToUseWhenUpdating.minutes:
    //            tMinutes = (float)dateTime.TotalNumberOfMinutesInDay / 1440f;
    //            //Debug.Log(tMinutes);
    //            return tMinutes;
    //        case TimeToUseWhenUpdating.hours:
    //            tHours = (float)dateTime.Hours / 24f;
    //            //Debug.Log(tHours);
    //            return tHours;

    //        default:
    //            return tHours;
    //    }
    //}

    //public void UpdateUI(DateTime dateTime)
    //{
    //    if (Date)
    //    {
    //        Date.text = dateTime.DateToString();
    //    }
    //    if (Time)
    //    {
    //        Time.text = dateTime.TimeAMPMToString();
    //    }
    //    if (Season)
    //    {
    //        Season.text = dateTime.Season.ToString();
    //    }
    //    if (Week)
    //    {
    //        Week.text = $"Week: {dateTime.CurrentWeek.ToString()}";
    //    }
    //}
}

//[System.Serializable]
//public enum TimeToUseWhenUpdating
//{
//    NULL = 0,
//    seconds = 1,
//    minutes = 2,
//    hours = 3
//}
