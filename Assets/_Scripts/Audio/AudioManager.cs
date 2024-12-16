using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;

    [Range(-0.2f, 0.2f)] public float pitchVariation;
    [Range(-0.2f, 0.2f)] public float volumeVariation;
    private void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound not found:" + name);
            return;
        }

        if (s.applyVariations)
        {
            s.source.pitch = s.pitch + UnityEngine.Random.Range(-pitchVariation, pitchVariation);
            s.source.volume = s.volume + UnityEngine.Random.Range(-volumeVariation, volumeVariation);
        }
        else
        {
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound not found:" + name);
            return;
        }

        s.source.Stop();
    }

}
