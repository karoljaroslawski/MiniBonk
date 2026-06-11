using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerXP : MonoBehaviour
{
    public int level = 1;

    public int currentXP = 0;

    public int xpToNextLevel = 50;

    public float xpMultiplier = 1f;

    public void AddXP(int amount)
    {
        currentXP += Mathf.RoundToInt(
            amount * xpMultiplier
        );

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UIManager.Instance.UpdateXP(
            currentXP,
            xpToNextLevel
        );
    }

    void LevelUp()
    {
        level++;

        currentXP -= xpToNextLevel;

        xpToNextLevel += 25;

        UpgradeManager.Instance.ShowUpgrade();

        UIManager.Instance.UpdateLevel(
            level
        );
    }

    void Update()
    {
        UIManager.Instance.UpdateStats(
            //GetComponent<AutoShooter>(),
            GetComponent<PlayerMovement>(),
            this,
            GetComponent<PlayerHealth>()
        );

        if (Keyboard.current.kKey.isPressed)
            AddXP(5);
    }
}