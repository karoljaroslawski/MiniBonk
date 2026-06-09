using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider healthBar;
    public Slider xpBar;

    public TMP_Text levelText;
    public TMP_Text waveText;
    public TMP_Text killsText;
    public TMP_Text waveTimerText;
    public TMP_Text healthText;
    public TMP_Text statsText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(
        int current,
        int max)
    {
        healthBar.maxValue = max;
        healthBar.value = current;

        healthText.text =
            "HP: " +
            current +
            " / " +
            max;
    }

    public void UpdateXP(
        int current,
        int max)
    {
        xpBar.maxValue = max;
        xpBar.value = current;
    }

    public void UpdateLevel(
        int level)
    {
        levelText.text =
            "Level: " + level;
    }

    public void UpdateWave(
        int wave)
    {
        waveText.text =
            "Wave: " + wave;
    }

    public void UpdateKills(
        int kills)
    {
        killsText.text =
            "Kills: " + kills;
    }

    public void UpdateWaveTimer(float time)
    {
        waveTimerText.text =
            "Next Wave: " +
            Mathf.CeilToInt(time) +
            "s";
    }

    public void UpdateStats(
        //AutoShooter shooter,
        PlayerMovement movement,
        PlayerXP xp,
        PlayerHealth health)
    {
        statsText.text =
            "STATS\n\n" +

            //"Damage: " +
            //shooter.damage +

            //"\nFire Rate: " +
            //shooter.fireRate.ToString("F2") +

            "\nSpeed: " +
            movement.speed.ToString("F1") +

            "\nHP Regen: " +
            health.healthRegen.ToString("F1") +
            "/s" +

            "\nXP Bonus: " +
            ((xp.xpMultiplier - 1f) * 100)
            .ToString("F0") +
            "%";
    }

    void Start()
    {
        UpdateKills(0);
        UpdateWave(1);
        UpdateLevel(1);
    }
}