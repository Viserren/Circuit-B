using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _ambienceSlider;
    [SerializeField] Slider _soundsSlider;

    private void OnEnable()
    {
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _ambienceSlider.onValueChanged.AddListener(SetAmbienceVolume);
        _soundsSlider.onValueChanged.AddListener(SetSoundsVolume);
    }

    private void OnDisable()
    {
        _masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        _musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        _ambienceSlider.onValueChanged.RemoveListener(SetAmbienceVolume);
        _soundsSlider.onValueChanged.RemoveListener(SetSoundsVolume);
    }

    public void LoadSliders()
    {
        _masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        _musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        _ambienceSlider.value = PlayerPrefs.GetFloat(AudioManager.AMBIENCE_KEY, 1f);
        _soundsSlider.value = PlayerPrefs.GetFloat(AudioManager.SOUNDS_KEY, 1f);
    }

    public void ApplyVolumeSettings()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, _masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, _musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.AMBIENCE_KEY, _ambienceSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SOUNDS_KEY, _soundsSlider.value);
    }

    void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat(AudioManager.MIXER_MASTER, Mathf.Log10(value) * 20);
    }

    void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat(AudioManager.MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetAmbienceVolume(float value)
    {
        _audioMixer.SetFloat(AudioManager.MIXER_AMBIENCE, Mathf.Log10(value) * 20);
    }

    void SetSoundsVolume(float value)
    {
        _audioMixer.SetFloat(AudioManager.MIXER_SOUNDS, Mathf.Log10(value) * 20);
    }
}
