using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponCard : MonoBehaviour
{
    public Image weaponIcon;
    public TMP_Text weaponLevel;
    public int level = 1;

    public void SetLevel(int level)
    {
        this.level = level;
        if (level >= 8)
            weaponLevel.text = "MAX";
        else
            weaponLevel.text = "LVL " + level.ToString();
    }
    
    public int GetLevel()
    {
        return this.level;
    }

    public void SetIcon(Sprite icon)
    {
        this.weaponIcon.sprite = icon;
    }
}
