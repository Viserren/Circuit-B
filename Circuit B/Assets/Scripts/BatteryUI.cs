using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateValue(float value)
    {
        _slider.value = value;
    }

    public void UpdateMaxValue(float value)
    {
        _slider.maxValue = value;
    }
}
