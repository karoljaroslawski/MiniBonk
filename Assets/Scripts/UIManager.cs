using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Bar healthBar;
    public Bar xpBar;

    public TMP_Text levelText;
    public TMP_Text waveText;
    public TMP_Text killsText;
    public TMP_Text waveTimerText;
    public TMP_Text healthText;
    public TMP_Text statsText;

    public WeaponManager weaponManager;
    public Transform weaponList;
    public GameObject weaponCardPrefab;
    public Sprite weaponSingleSprite;
    public Sprite weaponShotgunSprite;
    public Sprite weaponSniperSprite;
    public Sprite weaponSwordSprite;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(int current, int max)
    {
        healthText.text = current + " / " + max;
        healthBar.SetValue((float)current / (float)max);
    }

    public void UpdateXP(int current, int max)
    {
        xpBar.SetValue((float)current / (float)max);
    }

    public void UpdateLevel(int level)
    {
        levelText.text =
            "Level: " + level;
    }

    public void UpdateWave(int wave)
    {
        waveText.text =
            "Wave: " + wave;
    }

    public void UpdateKills(int kills)
    {
        killsText.text = kills.ToString();
    }

    public void UpdateWaveTimer(float time)
    {
        waveTimerText.text =
            "Next Wave: " +
            Mathf.CeilToInt(time) +
            "s";
    }

    public void UpdateStats(PlayerMovement movement, PlayerXP xp, PlayerHealth health)
    {
        statsText.text =
            "Speed: " +
            movement.speed.ToString("F1") +

            "\nHP Regen: " +
            health.healthRegen.ToString("F1") +
            "/s" +

            "\nXP Bonus: " +
            ((xp.xpMultiplier - 1f) * 100)
            .ToString("F0") +
            "%";
    }

    public void UpdateWeapons()
    {
        foreach (Transform child in weaponList)
        {
            Destroy(child.gameObject);
        }
        Func<WeaponTypes, Sprite> getWeaponSprite = wt =>
        {
            switch (wt)
            {
                case WeaponTypes.single: return weaponSingleSprite;
                case WeaponTypes.shotgun: return weaponShotgunSprite;
                case WeaponTypes.sniper: return weaponSniperSprite;
                case WeaponTypes.sword: return weaponSwordSprite;
            }
            return null;
        };

        List<Weapon> activeWeapons = weaponManager.GetActiveWeapons();
        foreach(Weapon w in activeWeapons)
        {
            GameObject card = Instantiate(weaponCardPrefab);
            card.GetComponent<WeaponCard>().SetIcon(getWeaponSprite(w.weaponType));
            card.GetComponent<WeaponCard>().SetLevel(w.level+1);
            card.transform.SetParent(weaponList);
        }
    }


    void Start()
    {
        UpdateKills(0);
        UpdateWave(1);
        UpdateLevel(1);
        UpdateWeapons();
    }
}