using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static float sensitivity = 1f;
    public static float SFXvolume = 1f;
    public static float Musicvolume = 1f;
    public AudioMixer mainMixer;

    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1f);
        SFXvolume = PlayerPrefs.GetFloat("SFXvolume", 1f);
        Musicvolume = PlayerPrefs.GetFloat("Musicvolume", 1f);

        //AudioListener.volume = volume;
        SetSFXVolume(SFXvolume);
        SetMusicVolume(Musicvolume);
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    public void SetSFXVolume(float value)
    {
        SFXvolume = value;
        SetSFX(SFXvolume);
        PlayerPrefs.SetFloat("SFXvolume", value);
    }
    public void SetMusicVolume(float value)
    {
        Musicvolume = value;
        SetMusic(Musicvolume);
        PlayerPrefs.SetFloat("Musicvolume", value);
    }
    void SetSFX(float volumeValue)
    {
        if (volumeValue <= 0.01f)
        {
            mainMixer.SetFloat("SFXParam", -80f);
        }
        else
        {
            float dbValue = Mathf.Log10(volumeValue) * 20;
            mainMixer.SetFloat("SFXParam", dbValue);
        }
    }
    void SetMusic(float volumeValue)
    {
        if (volumeValue <= 0.01f)
        {
            mainMixer.SetFloat("MusicParam", -80f);
        }
        else
        {
            float dbValue = Mathf.Log10(volumeValue) * 20;
            mainMixer.SetFloat("MusicParam", dbValue);
        }
    }
}