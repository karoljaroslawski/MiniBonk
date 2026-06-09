using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;
    public float healthRegen = 0.1f;


    private float regenAccumulator;

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
        currentHealth -= damage;

        Debug.Log("HP: " + currentHealth);



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
        Debug.Log("GAME OVER");

        gameObject.SetActive(false);
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