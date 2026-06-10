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
    public WeaponManager weaponManager;

    public AudioSource audioSource;
    public AudioClip audioLevelUp;

    void Awake()
    {
        Instance = this;
    }

    public void ShowUpgrade()
    {
        audioSource.PlayOneShot(audioLevelUp, 0.1f);

        Debug.Log("SHOW UPGRADE");

        Time.timeScale = 0f;

        panel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GenerateChoices();
    }

    void GenerateChoices()
    {
        List<WeaponTypes> aviableUpgrades = weaponManager.GetComponent<WeaponManager>().GetAviableUpgrades();

        List<UpgradeType> upgrades =
            new List<UpgradeType>()
            {
                UpgradeType.MaxHealth,
                UpgradeType.Heal,
                UpgradeType.XPMultiplier,
                UpgradeType.MoveSpeed,
                UpgradeType.HealthRegen
            };
        if (aviableUpgrades.Contains(WeaponTypes.single)) upgrades.Add(UpgradeType.WeaponSingle);
        if (aviableUpgrades.Contains(WeaponTypes.shotgun)) upgrades.Add(UpgradeType.WeaponShotgun);
        if (aviableUpgrades.Contains(WeaponTypes.sniper)) upgrades.Add(UpgradeType.WeaponSniper);
        if (aviableUpgrades.Contains(WeaponTypes.sword)) upgrades.Add(UpgradeType.WeaponSword);

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

            case UpgradeType.WeaponSingle:
                return "Weapon magic bolt";

            case UpgradeType.WeaponShotgun:
                return "Weapon fire wave";

            case UpgradeType.WeaponSniper:
                return "Weapon spark";

            case UpgradeType.WeaponSword:
                return "Weapon sword";
        }

        return "";
    }


    void SelectUpgrade(
        UpgradeType type)
    {

        ApplyUpgrade(type);

        panel.SetActive(false);


        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ApplyUpgrade(
        UpgradeType type)
    {
        GameObject player =
            GameObject.Find("Player");

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        PlayerXP xp =
            player.GetComponent<PlayerXP>();

        PlayerMovement move =
            player.GetComponent<PlayerMovement>();

        switch (type)
        {

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

            case UpgradeType.WeaponSingle:
                weaponManager.UpgradeWeapon(WeaponTypes.single);
                break;
            case UpgradeType.WeaponShotgun:
                weaponManager.UpgradeWeapon(WeaponTypes.shotgun);
                break;
            case UpgradeType.WeaponSniper:
                weaponManager.UpgradeWeapon(WeaponTypes.sniper);
                break;
            case UpgradeType.WeaponSword:
                weaponManager.UpgradeWeapon(WeaponTypes.sword);
                break;
        }
    }
}