using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public TMP_Text battleFocusCooldown;
    public TMP_Text arcaneShieldCooldown;
    public TMP_Text timeWarpCooldown;

    public Image battleFocusIcon;
    public Image arcaneShieldIcon;
    public Image timeWarpIcon;

    void Update()
    {
        if (AbilityManager.Instance == null)
            return;

        UpdateCooldown(
            battleFocusCooldown,
            AbilityManager.Instance.BattleFocusCooldownRemaining
        );

        UpdateCooldown(
            arcaneShieldCooldown,
            AbilityManager.Instance.ArcaneShieldCooldownRemaining
        );

        UpdateCooldown(
            timeWarpCooldown,
            AbilityManager.Instance.TimeWarpCooldownRemaining
        );

        UpdateIcon(
            battleFocusIcon,
            AbilityManager.Instance.BattleFocusCooldownRemaining
        );

        UpdateIcon(
            arcaneShieldIcon,
            AbilityManager.Instance.ArcaneShieldCooldownRemaining
        );

        UpdateIcon(
            timeWarpIcon,
            AbilityManager.Instance.TimeWarpCooldownRemaining
        );
    }

    void UpdateCooldown(TMP_Text text, float cooldown)
    {
        if (cooldown <= 0)
        {
            text.text = "";
        }
        else
        {
            text.text = Mathf.CeilToInt(cooldown).ToString();
        }
    }

    void UpdateIcon(Image image, float cooldown)
    {
        if (image == null)
            return;

        if (cooldown > 0)
        {
            image.color = new Color(
                0.25f,
                0.25f,
                0.25f,
                1f
            );
        }
        else
        {
            image.color = Color.white;
        }
    }
}