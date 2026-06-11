using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject canvasPanel;

    public Slider sensSlider;
    public Slider volumeSlider;

    public void Start()
    {
        int bestkills = PlayerPrefs.GetInt("kills", 0);
        int bestlevel = PlayerPrefs.GetInt("level", 0);
        int bestwave = PlayerPrefs.GetInt("wave", 0);
        float besttime = PlayerPrefs.GetFloat("time", 0f);

        canvasPanel.transform.Find("Best kills").GetComponent<TMP_Text>().text = "kills: " + bestkills;
        canvasPanel.transform.Find("Best score").GetComponent<TMP_Text>().text = "xp level: " + bestlevel;
        canvasPanel.transform.Find("Best wave").GetComponent<TMP_Text>().text = "wave: " + bestwave;
        int bestminutes = Mathf.FloorToInt(besttime / 60f);
        int bestseconds = Mathf.FloorToInt(besttime % 60f);
        canvasPanel.transform.Find("Best time").GetComponent<TMP_Text>().text = "time: " + $"{bestminutes:00}:{bestseconds:00}";
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
        volumeSlider.SetValueWithoutNotify(SettingsManager.volume);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
