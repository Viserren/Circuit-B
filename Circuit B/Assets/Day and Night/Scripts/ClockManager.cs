using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    //public RectTransform ClockFace;
    //public TextMeshProUGUI Date, Time, Season, Week;

    [SerializeField] AudioMixerSnapshot _daySnapshot, _nightSnapshot;
    [SerializeField] Color dayColour, nightColour;

    //public Image weatherSprite;
    //public Sprite[] weatherSprites;

    private float sunStartingRotation;

    public Light sunLight;
    public float dayIntensity;
    public float nightIntensity;
    //public TimeToUseWhenUpdating timeToUseWhenUpdating;
    public AnimationCurve dayNightCurve;
    public AnimationCurve sunHeightCurve;
    bool _isDayNext;

    //float tSeconds;
    //float tMinutes;
    //float tHours;

    [SerializeField] int _secondsToAdd;
    [Tooltip("1 Second is 1 Seconds in real life, so 0.5 will be 2x as fast as real time")]
    [SerializeField] float _timeInterval;
    [SerializeField] int _totalTimeInDay = 43200;
    float linierTime;
    public int secondsToAdd { get { return _secondsToAdd; } set { _secondsToAdd = value; } }

    int _totalSeconds;
    int _secondsAim;

    Coroutine _moveTime;

    private void Awake()
    {
        //startingRotation = ClockFace.eulerAngles.z;
        sunStartingRotation = sunLight.transform.eulerAngles.z;
    }

    private void Start()
    {
        _isDayNext = linierTime > .25f && linierTime < .75f ? true : false;
    }

    private void FixedUpdate()
    {
        //UpdateRealDateTime();
        UpdateTime();
    }

    public void UpdateTime()
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        Calendar cal = dfi.Calendar;

        #region Sun Angle
        int currenWeek = cal.GetWeekOfYear(System.DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        float pos = (float)currenWeek / 52f;
        #endregion

        // Default 86400f
        linierTime = (float)_totalSeconds / (float)_totalTimeInDay;
        //Debug.Log($"0 To 1 Time: {linierTime}, 0 To 1 Week: {pos} Seconds: {_totalSeconds}");
        float newRotation = Mathf.Lerp(-180, 180, linierTime);

        if (linierTime > .25f && linierTime < .75f)
        {
            if (_isDayNext)
            {
                _daySnapshot.TransitionTo(20);
                _isDayNext = false;
            }
        }
        else
        {
            if (!_isDayNext)
            {
                _nightSnapshot.TransitionTo(20);
                _isDayNext = true;
            }
        }
        //40 32 31
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
        RenderSettings.fogColor = Color.Lerp(nightColour, dayColour, sunIntensity);
    }

    public void AddSeconds(int seconds)
    {
        _secondsAim += seconds;
        if (_moveTime == null)
        {
            _moveTime = StartCoroutine(UpdateSeconds());
        }
    }

    public void ResetTime()
    {
        if (_moveTime != null)
        {
            StopCoroutine(_moveTime);
            _moveTime = null;
        }
        _totalSeconds = 0;
    }

    IEnumerator UpdateSeconds()
    {
        for (int i = 0; i < _secondsAim; i++)
        {
            _totalSeconds++;
            //Debug.Log($"Total Seconds: {_totalSeconds} : Time Interval: {_timeInterval}");
            yield return new WaitForSeconds(_timeInterval);
        }
        _moveTime = null;
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
        //Debug.Log($"0 To 1 Time: {linierTime}, 0 To 1 Week: {pos} Seconds: {_totalSeconds}");
        float newRotation = Mathf.Lerp(-180, 180, linierTime);

        if (linierTime > .25f && linierTime < .75f)
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
            RenderSettings.fogColor = Color.Lerp(dayColour, nightColour, sunPos);
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
#if UNITY_EDITOR
[CustomEditor(typeof(ClockManager))]
public class ClockManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ClockManager _target = (ClockManager)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("AdvanceTime"))
        {
            _target.AddSeconds(_target.secondsToAdd);
            _target.secondsToAdd = 0;
        }
    }
}
#endif

//[System.Serializable]
//public enum TimeToUseWhenUpdating
//{
//    NULL = 0,
//    seconds = 1,
//    minutes = 2,
//    hours = 3
//}
