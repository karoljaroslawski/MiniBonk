using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;
    public float healthRegen = 0.1f;


    private float regenAccumulator;

    public AudioSource audioSource;
    public AudioClip audioHurt;

    public GameObject deathPanel;
    public GameObject UI;

    public bool isDead = false;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(
            currentHealth,
            maxHealth
        );
    }

    public void TakeDamage(int damage)
    {
        audioSource.PlayOneShot(audioHurt);

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        UIManager.Instance.UpdateHealth(
            currentHealth,
            maxHealth
        );
    }

    void Die()
    {
        //gameObject.SetActive(false);
        isDead=true;
        deathPanel.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        int kills = GameManager.Instance.kills;
        int level = gameObject.GetComponent<PlayerXP>().level;
        int wave = WaveManager.Instance.wave;
        float time = Time.time - WaveManager.Instance.startTime;

        deathPanel.transform.Find("Panel/kills").GetComponent<TMP_Text>().text = "kills: " + kills;
        deathPanel.transform.Find("Panel/score").GetComponent<TMP_Text>().text = "xp level: " + level;
        deathPanel.transform.Find("Panel/wave").GetComponent<TMP_Text>().text = "wave: " + wave;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        deathPanel.transform.Find("Panel/time").GetComponent<TMP_Text>().text = "time: " + $"{minutes:00}:{seconds:00}";

        int bestkills = PlayerPrefs.GetInt("kills", 0);
        int bestlevel = PlayerPrefs.GetInt("level", 0);
        int bestwave = PlayerPrefs.GetInt("wave", 0);
        float besttime = PlayerPrefs.GetFloat("time", 0f);

        deathPanel.transform.Find("Panel/Best kills").GetComponent<TMP_Text>().text = "kills: " + bestkills;
        deathPanel.transform.Find("Panel/Best score").GetComponent<TMP_Text>().text = "xp level: " + bestlevel;
        deathPanel.transform.Find("Panel/Best wave").GetComponent<TMP_Text>().text = "wave: " + bestwave;
        int bestminutes = Mathf.FloorToInt(besttime / 60f);
        int bestseconds = Mathf.FloorToInt(besttime % 60f);
        deathPanel.transform.Find("Panel/Best time").GetComponent<TMP_Text>().text = "time: " + $"{bestminutes:00}:{bestseconds:00}";

        if (wave > bestwave || (wave == bestwave && kills > bestkills))
        {
            deathPanel.transform.Find("Panel/Best result").GetComponent<TMP_Text>().text = "New Record!\nPrevious best score:";
            PlayerPrefs.SetFloat("time", time);
            PlayerPrefs.SetInt("kills", kills);
            PlayerPrefs.SetInt("wave", wave);
            PlayerPrefs.SetInt("level", level);
        }
    }

    public void IncreaseMaxHealth(
    int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        UIManager.Instance.UpdateHealth(
            currentHealth,
            maxHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UIManager.Instance.UpdateHealth(
            currentHealth,
            maxHealth
        );
    }



    void Update()
    {
        regenAccumulator +=
            healthRegen * Time.deltaTime;

        if (regenAccumulator >= 1f)
        {
            int healAmount =
                Mathf.FloorToInt(regenAccumulator);

            regenAccumulator -= healAmount;

            Heal(healAmount);
        }
    }
}