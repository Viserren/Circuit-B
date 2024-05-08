using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    public Sound[] allMusic;
    public Sound[] allSounds;


    [Header("Background Music")]
    Sound _currentPlayerMusic;
    int _currentMusicIndex = 0;

    [SerializeField] AudioMixerGroup _menuMusicMixer;

    [SerializeField] AudioMixerSnapshot _menuMusic, _inGameMusic;
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

        foreach (Sound s in allMusic)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.outputAudioMixerGroup = s.mixerGroup;
        }

        foreach (Sound s in allSounds)
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
    }

    public void PlayMusic(string name)
    {
        try
        {
            Debug.Log("Playing Music");
            _currentPlayerMusic = Array.Find(allMusic, sound => sound.name == name);
            _currentPlayerMusic.audioSource.outputAudioMixerGroup = _currentPlayerMusic.mixerGroup;
            _currentPlayerMusic.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void TransitionToMenuMusic(float time)
    {
        _menuMusic.TransitionTo(time);
        if (_currentPlayerMusic != null)
        {
            _currentPlayerMusic.mixerGroup = _menuMusicMixer;
        }
        else
        {
            PlayRandomSong();
        }
    }

    public void TransitionToInGameMusic(float time)
    {
        _inGameMusic.TransitionTo(time);
    }

    public void PlayRandomSong()
    {
        if (_currentPlayerMusic == null)
        {
            try
            {
                _currentMusicIndex = Random.Range(0, allMusic.Length);
                _currentPlayerMusic = allMusic[_currentMusicIndex];
                _currentPlayerMusic.audioSource.outputAudioMixerGroup = _menuMusicMixer;
                _currentPlayerMusic.audioSource.Play();
            }
            catch (Exception e)
            {
                Debug.Log($"{e.Message}");
                throw;
            }
        }

    }

    public void PlayNextSong()
    {
        _currentMusicIndex++;
        if (_currentMusicIndex >= allMusic.Length)
        {
            _currentMusicIndex = 0;
        }
        try
        {
            _currentPlayerMusic = allMusic[_currentMusicIndex];
            _currentPlayerMusic.audioSource.outputAudioMixerGroup = _menuMusicMixer;
            _currentPlayerMusic.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public void PlayPreviousSong()
    {
        _currentMusicIndex--;
        if (_currentMusicIndex < 0)
        {
            _currentMusicIndex = allMusic.Length;
        }
        try
        {
            _currentPlayerMusic = allMusic[_currentMusicIndex];
            _currentPlayerMusic.audioSource.outputAudioMixerGroup = _menuMusicMixer;
            _currentPlayerMusic.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }


    public void PlayMusicInMenu(string name)
    {
        try
        {
            _currentPlayerMusic = Array.Find(allMusic, sound => sound.name == name);
            _currentPlayerMusic.audioSource.outputAudioMixerGroup = _menuMusicMixer;
            _currentPlayerMusic.audioSource.Play();
        }
        catch (Exception e)
        {
            Debug.Log($"{e.Message}");
            throw;
        }
    }

    public async void StopMusicInMenu(int time)
    {
        if (_currentPlayerMusic != null)
        {
            try
            {
                _inGameMusic.TransitionTo(time);
                await Task.Delay(time * 1000);
                _currentPlayerMusic.audioSource.Stop();
                _currentPlayerMusic = null;
            }
            catch (Exception e)
            {
                Debug.Log($"{e.Message}");
                throw;
            }
        }
    }

    public void PlaySound(string name)
    {
        try
        {
            Sound s = Array.Find(allSounds, sound => sound.name == name);
            s.audioSource.Play();
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
