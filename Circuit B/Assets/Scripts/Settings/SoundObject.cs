using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] SoundEmissions[] _soundEmissions;

    public SoundEmissions[] SoundEmissions { get { return _soundEmissions; } }

    private void Start()
    {
        SetupSounds();
    }

    public Sound[] GetSoundArray(string name)
    {
        Sound[] sound = System.Array.Find(_soundEmissions, sound => sound.soundsName == name).sounds;
        return sound;
    }

    void SetupSounds()
    {
        foreach (SoundEmissions se in _soundEmissions)
        {
            foreach (Sound s in se.sounds)
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
    }
}

[System.Serializable]
public class SoundEmissions
{
    public string soundsName;
    public Sound[] sounds;
}