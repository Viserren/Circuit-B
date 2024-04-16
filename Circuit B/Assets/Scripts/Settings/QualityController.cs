using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QualityController : MonoBehaviour
{
    public const string CURRENTQUALITY_KEY = "CurrentQualityKey";
    int _quality;
    [SerializeField] TMP_Dropdown _qualityDropDown;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(CURRENTQUALITY_KEY))
        {
            SetFirstLoadQualityLevel();
        }
        LoadQualitySetting();
    }

    public void SwitchQualityLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
    }

    void ChangeAssetProperties()
    {

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
}
