using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static float sensitivity = 1f;
    public static float volume = 1f;

    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity", 1f);
        volume = PlayerPrefs.GetFloat("volume", 1f);

        AudioListener.volume = volume;
    }

    public void SetSensitivity(float value)
    {
        sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    public void SetVolume(float value)
    {
        volume = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("volume", value);
    }
}