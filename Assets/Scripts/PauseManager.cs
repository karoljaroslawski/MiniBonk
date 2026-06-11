using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject UI;
    public GameObject pausePanel;
    public GameObject menuPanel;
    public GameObject settingsPanel;

    public Slider sensSlider;
    public Slider volumeSlider;
    public Slider musicSlider;

    private bool isPaused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        if (GameObject.Find("Player").GetComponent<PlayerHealth>().isDead == false) {
            pausePanel.SetActive(true);
            UI.SetActive(false);
            Time.timeScale = 0f;
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        UI.SetActive(true);
        isPaused = false;
        if (!upgradePanel.activeSelf) { 
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }
    public void OpenSettings()
    {
        sensSlider.SetValueWithoutNotify(SettingsManager.sensitivity);
        volumeSlider.SetValueWithoutNotify(SettingsManager.SFXvolume);
        musicSlider.SetValueWithoutNotify(SettingsManager.Musicvolume);
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
