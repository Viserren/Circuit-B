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
    int _resWidth;
    int _resHeight;
    double _refreshRate;

    Resolution[] _resolutions;
    RefreshRate[] _refreshRates;

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
        Debug.Log($"Value: {Screen.currentResolution.refreshRateRatio.value} \n Numerator: {Screen.currentResolution.refreshRateRatio.numerator} \n Denominator: {Screen.currentResolution.refreshRateRatio.denominator}");
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
        _resWidth = PlayerPrefs.GetInt(CURRENTRESOLUTIONWIDTH_KEY, Screen.currentResolution.width);
        _resHeight = PlayerPrefs.GetInt(CURRENTRESOLUTIONHEIGHT_KEY, Screen.currentResolution.height);

        _resolutionsDropDown.ClearOptions();
        List<string> resolutions = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            resolutions.Add($"{_resolutions[i].width} x {_resolutions[i].height}");
            Debug.Log($"{_resolutions[i].width} x {_resolutions[i].height}");
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
    }
    #endregion

    #region RefreshRate
    public void SwitchRefreshRate(int index)
    {
        Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, _refreshRates[index]);
    }

    public void SaveRefreshRateSetting(int index)
    {
        PlayerPrefs.SetInt(CURRENTREFRESHRATE_KEY, Convert.ToInt32(_refreshRates[index].value));
    }

    void LoadRefreshSetting()
    {
        _resolutions = Screen.resolutions;
        _refreshRate = PlayerPrefs.GetInt(CURRENTREFRESHRATE_KEY, Convert.ToInt32(Screen.currentResolution.refreshRateRatio.value));

        _resolutionsDropDown.ClearOptions();
        List<string> refreshrates = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (!refreshrates.Contains($"{_resolutions[i].refreshRateRatio.value}Hz"))
            {
                refreshrates.Add($"{_resolutions[i].refreshRateRatio.value}Hz");

            }
        }
        _refreshRateDropDown.AddOptions(refreshrates);
        for (int i = 0; i < _resolutions.Length; i++)
        {
            if (_refreshRate == _resolutions[i].refreshRateRatio.value)
            {
                _refreshRateDropDown.value = i;
            }
        }
        _refreshRateDropDown.RefreshShownValue();
    }
    #endregion

    #region Fullscreen

    #endregion
}
