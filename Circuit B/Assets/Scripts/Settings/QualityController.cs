using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QualityController : MonoBehaviour
{
    public const string CURRENTQUALITY_KEY = "CurrentQualityKey";
    public const string CURRENTRESOLUTIONWIDTH_KEY = "CurrentResolutionWidthKey";
    public const string CURRENTRESOLUTIONHEIGHT_KEY = "CurrentResolutionHeightKey";
    public const string CURRENTREFRESHRATE_KEY = "CurrentRefreshRateKey";

    int _quality;
    int _refreshRate;

    Resolution[] _resolutions;
    List<int> _refreshRates = new List<int>();

    [SerializeField] TMP_Dropdown _qualityDropDown;
    [SerializeField] TMP_Dropdown _resolutionsDropDown;
    [SerializeField] TMP_Dropdown _refreshRateDropDown;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(CURRENTQUALITY_KEY))
        {
            SetFirstLoadQualityLevel();
        }
        LoadQualitySetting();
        LoadResolutionSetting();
        //LoadRefreshSetting();
        //Debug.Log($"Value: {Screen.currentResolution.refreshRateRatio.value} \n Numerator: {Screen.currentResolution.refreshRateRatio.numerator} \n Denominator: {Screen.currentResolution.refreshRateRatio.denominator}");
    }

    #region Quality

    public void SwitchQualityLevel(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SaveQualitySetting()
    {
        PlayerPrefs.SetInt(CURRENTQUALITY_KEY, _qualityDropDown.value);
    }

    void LoadQualitySetting()
    {
        _quality = PlayerPrefs.GetInt(CURRENTQUALITY_KEY, QualitySettings.GetQualityLevel());
        Debug.Log($"Quality: {_quality}");
        _qualityDropDown.value = _quality;
        _qualityDropDown.RefreshShownValue();
    }

    void SetFirstLoadQualityLevel()
    {
        switch (SystemInfo.graphicsMemorySize)
        {
            case <= 2048:
                QualitySettings.SetQualityLevel(1);

                break;
            case <= 4048:
                QualitySettings.SetQualityLevel(2);
                break;
            default:
                QualitySettings.SetQualityLevel(0);
                break;
        }
        int temp = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt(CURRENTQUALITY_KEY, temp);
    }
    #endregion

    #region Resolution
    public void SwitchResolution(int index)
    {
        Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, FullScreenMode.FullScreenWindow);
    }
    public void SaveResolutionSetting()
    {
        PlayerPrefs.SetInt(CURRENTRESOLUTIONWIDTH_KEY, Screen.currentResolution.width);
        PlayerPrefs.SetInt(CURRENTRESOLUTIONHEIGHT_KEY, Screen.currentResolution.height);
    }

    void LoadResolutionSetting()
    {
        _resolutions = Screen.resolutions;
        int _resWidth = PlayerPrefs.GetInt(CURRENTRESOLUTIONWIDTH_KEY, Screen.currentResolution.width);
        int _resHeight = PlayerPrefs.GetInt(CURRENTRESOLUTIONHEIGHT_KEY, Screen.currentResolution.height);

        _resolutionsDropDown.ClearOptions();
        List<string> resolutions = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            resolutions.Add($"{_resolutions[i].width} x {_resolutions[i].height}");
            //Debug.Log($"{_resolutions[i].width} x {_resolutions[i].height}");
        }
        _resolutionsDropDown.AddOptions(resolutions);
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_resWidth == _resolutions[i].width && _resHeight == _resolutions[i].height)
            {
                _resolutionsDropDown.value = i;
            }
        }
        _resolutionsDropDown.RefreshShownValue();

        LoadRefreshSetting();
    }
    #endregion

    #region RefreshRate
    public void SwitchRefreshRate(int index)
    {
        Application.targetFrameRate = _refreshRates[index];
        _refreshRate = _refreshRates[index];
    }

    public void SaveRefreshRateSetting()
    {
        PlayerPrefs.SetInt(CURRENTREFRESHRATE_KEY, _refreshRate);
    }

    void LoadRefreshSetting()
    {
        _refreshRate = PlayerPrefs.GetInt(CURRENTREFRESHRATE_KEY, Convert.ToInt32(Screen.currentResolution.refreshRateRatio.value));

        _refreshRateDropDown.ClearOptions();
        int[] tempRefreshRates = new int[] { 30, 60, 120, 144, 240, 360 };
        List<string> refreshrates = new List<string>();
        for (int i = 0; i < tempRefreshRates.Length; i++)
        {
            if (tempRefreshRates[i] < Convert.ToInt32(Screen.currentResolution.refreshRateRatio.value))
            {
                _refreshRates.Add(tempRefreshRates[i]);
                refreshrates.Add($"{tempRefreshRates[i].ToString()}Hz");
            }
            else
            {
                _refreshRates.Add(Convert.ToInt32(Screen.currentResolution.refreshRateRatio.value));
                refreshrates.Add($"{Convert.ToInt32(Screen.currentResolution.refreshRateRatio.value)}Hz");
                break;
            }
        }

        _refreshRateDropDown.AddOptions(refreshrates);

        for (int i = 0; i < _refreshRates.Count; i++)
        {
            if (_refreshRate == _refreshRates[i])
            {
                _refreshRateDropDown.value = i;
            }
        }
        Application.targetFrameRate = _refreshRate;
        _refreshRateDropDown.RefreshShownValue();
    }
    #endregion

    #region Fullscreen

    #endregion
}
