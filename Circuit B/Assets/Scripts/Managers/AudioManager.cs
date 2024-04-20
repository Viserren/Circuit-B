using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    public Sound[] sounds;


    [Header("Background Music")]
    Sound _currentPlayerMusic;
    Sound[] _backgroundMusic;
    [SerializeField] float _secondsBetweenMusic = 60;
    public static AudioManager Instance { get; private set; }

    public const string MASTER_KEY = "MasterKey";
    public const string MUSIC_KEY = "MusicKey";
    public const string AMBIENCE_KEY = "AmbienceKey";
    public const string SOUNDS_KEY = "SoundsKey";

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_AMBIENCE = "AmbienceVolume";
    public const string MIXER_SOUNDS = "SoundsVolume";

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
        }
    }

    public void Start()
    {
        LoadVolumes();
        LoadBackgroundMusic();
        StartCoroutine(PlayBackgroundMusic());
    }

    public void Play(string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    IEnumerator PlayBackgroundMusic()
    {
        int temp = Random.Range(0, _backgroundMusic.Length);
        if (_currentPlayerMusic != null)
        {
            if (!_currentPlayerMusic.audioSource.isPlaying)
            {
                _currentPlayerMusic = _backgroundMusic[temp];
                Play(_currentPlayerMusic.name);
            }
        }
        else
        {
            _currentPlayerMusic = _backgroundMusic[temp];
            Play(_currentPlayerMusic.name);
        }

        yield return new WaitForSeconds(_secondsBetweenMusic);
    }

    void LoadBackgroundMusic()
    {
        try
        {
            _backgroundMusic = Array.FindAll(sounds, sound => sound.isMainBackgroundMusic == true);
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    void LoadVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float ambienceVolume = PlayerPrefs.GetFloat(AMBIENCE_KEY, 1f);
        float soundsVolume = PlayerPrefs.GetFloat(SOUNDS_KEY, 1f);

        _audioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        _audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        _audioMixer.SetFloat(MIXER_AMBIENCE, Mathf.Log10(ambienceVolume) * 20);
        _audioMixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(soundsVolume) * 20);
    }
}
