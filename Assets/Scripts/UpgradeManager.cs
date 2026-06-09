using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public GameObject panel;

    public Button button1;
    public Button button2;
    public Button button3;

    private UpgradeType[] currentChoices;

    void Awake()
    {
        Instance = this;
    }

    public void ShowUpgrade()
    {
        Debug.Log("SHOW UPGRADE");

        Time.timeScale = 0f;

        panel.SetActive(true);

        GenerateChoices();
    }

    void GenerateChoices()
    {
        List<UpgradeType> upgrades =
            new List<UpgradeType>()
            {
                UpgradeType.Damage,
                UpgradeType.FireRate,
                UpgradeType.MaxHealth,
                UpgradeType.Heal,
                UpgradeType.XPMultiplier,
                UpgradeType.MoveSpeed,
                UpgradeType.HealthRegen
            };

        currentChoices =
            new UpgradeType[3];

        for (int i = 0; i < 3; i++)
        {
            int random =
                Random.Range(
                    0,
                    upgrades.Count
                );

            currentChoices[i] =
                upgrades[random];

            upgrades.RemoveAt(random);
        }

        SetupButton(

            button1,
            currentChoices[0]
        );

        SetupButton(
            button2,
            currentChoices[1]
        );

        SetupButton(
            button3,
            currentChoices[2]
        );
    }

    void SetupButton(
        Button button,
        UpgradeType type)
    {
        Debug.Log("Setup: " + type);

        button.GetComponentInChildren<TMP_Text>().text =
            GetUpgradeText(type);

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(
            () => SelectUpgrade(type)
        );
    }

    string GetUpgradeText(
        UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Damage:
                return "+20% DAMAGE";

            case UpgradeType.FireRate:
                return "+15% FIRE RATE";

            case UpgradeType.MaxHealth:
                return "+20 MAX HP";

            case UpgradeType.Heal:
                return "HEAL 30 HP";

            case UpgradeType.XPMultiplier:
                return "+15% XP";

            case UpgradeType.MoveSpeed:
                return "+1 SPEED";

            case UpgradeType.HealthRegen:
                return "+0.1 HP REGEN";
        }

        return "";
    }


    void SelectUpgrade(
        UpgradeType type)
    {

        ApplyUpgrade(type);

        panel.SetActive(false);

        Time.timeScale = 1f;
    }

    void ApplyUpgrade(
        UpgradeType type)
    {
        GameObject player =
            GameObject.Find("Player");

        //AutoShooter shooter =
         //   player.GetComponent<AutoShooter>();

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        PlayerXP xp =
            player.GetComponent<PlayerXP>();

        PlayerMovement move =
            player.GetComponent<PlayerMovement>();

        switch (type)
        {
            /*case UpgradeType.Damage:

                shooter.damage =
                    Mathf.RoundToInt(
                        shooter.damage * 1.2f
                    );
                break;

            case UpgradeType.FireRate:

                shooter.fireRate *= 0.85f;
                break;*/

            case UpgradeType.MaxHealth:

                health.maxHealth += 20;
                UIManager.Instance.UpdateHealth(
                    health.CurrentHealth,
                    health.maxHealth
                );
                break;

            case UpgradeType.Heal:

                health.Heal(30);
                break;

            case UpgradeType.XPMultiplier:

                xp.xpMultiplier *= 1.15f;
                break;

            case UpgradeType.MoveSpeed:

                move.speed += 1f;
                break;

            case UpgradeType.HealthRegen:

                health.healthRegen += 0.1f;

                break;
        }
    }
}