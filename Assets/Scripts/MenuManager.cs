using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject canvasPanel;
    public GameObject bestPanel;

    public Slider sensSlider;
    public Slider volumeSlider;
    public Slider musicSlider;

    public void Start()
    {
        int bestkills = PlayerPrefs.GetInt("kills", 0);
        int bestlevel = PlayerPrefs.GetInt("level", 0);
        int bestwave = PlayerPrefs.GetInt("wave", 0);
        float besttime = PlayerPrefs.GetFloat("time", 0f);

        canvasPanel.transform.Find("Best/Best kills").GetComponent<TMP_Text>().text = "kills: " + bestkills;
        canvasPanel.transform.Find("Best/Best score").GetComponent<TMP_Text>().text = "xp level: " + bestlevel;
        canvasPanel.transform.Find("Best/Best wave").GetComponent<TMP_Text>().text = "wave: " + bestwave;
        int bestminutes = Mathf.FloorToInt(besttime / 60f);
        int bestseconds = Mathf.FloorToInt(besttime % 60f);
        canvasPanel.transform.Find("Best/Best time").GetComponent<TMP_Text>().text = "time: " + $"{bestminutes:00}:{bestseconds:00}";
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {
        sensSlider.SetValueWithoutNotify(SettingsManager.sensitivity);
        volumeSlider.SetValueWithoutNotify(SettingsManager.SFXvolume);
        musicSlider.SetValueWithoutNotify(SettingsManager.Musicvolume);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        bestPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        bestPanel.SetActive(true);
    }
}
