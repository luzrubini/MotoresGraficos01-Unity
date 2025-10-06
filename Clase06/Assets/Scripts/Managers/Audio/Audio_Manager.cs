using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class Audio_Manager : MonoBehaviour
{
    public Sound[] sounds;
    //Scene currentScene;

    public static Audio_Manager instance;

    private IEnumerator fadeOut;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Menu");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        if(s.type == Sound.TypeOfSound.Music)
        {
            s.source.volume = PlayerPrefs.GetFloat("Volume");
        }
        else if(s.type == Sound.TypeOfSound.SFXs)
        {
            s.source.volume = PlayerPrefs.GetFloat("SFXs");
        }
        
        s.source.Play();
    }

    public IEnumerator Stop(string name)
    {
        yield return new WaitForSeconds(.3f);
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            yield return null;
        fadeOut = FadeOut(s.source, .3f, 0f);
        StartCoroutine(fadeOut);
    }

    public IEnumerator FadeOut(AudioSource audioSource, float duration, float targetVolume)
    {
        float timer = 0;
        float currentVolume = audioSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, 0f, 1f);

        audioSource.volume = 1f;

        while (audioSource.volume > 0)
        {
            timer += Time.deltaTime;
            var newVolume = Mathf.Lerp(currentVolume, targetValue, timer / duration);
            audioSource.volume = newVolume;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 1f;
    }

    public void ChangeVolumeMusic(float volume)
    {
        foreach (Sound s in sounds)
        {
            if(s.type == Sound.TypeOfSound.Music)
            {
                s.volume = volume;
                PlayerPrefs.SetFloat("Volume", volume);

                foreach (AudioSource AudioS in gameObject.GetComponents<AudioSource>())
                {
                    if (AudioS.clip.name == s.name)
                    {
                        AudioS.volume = volume;
                    }
                }
            }
        }
        
    }

    public void ChangeVolumeSFXs(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.type == Sound.TypeOfSound.SFXs)
            {
                s.volume = volume;
                PlayerPrefs.SetFloat("SFXs", volume);

                foreach (AudioSource AudioS in gameObject.GetComponents<AudioSource>())
                {
                    if (AudioS.clip.name == s.name)
                    {
                        AudioS.volume = volume;
                    }
                }
            }
        }

    }
}
