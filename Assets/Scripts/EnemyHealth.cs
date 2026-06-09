using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyType enemyType;

    public int cost = 1;

    [Header("Base Stats")]
    public int baseHealth = 20;
    public int baseXPReward = 10;

    [HideInInspector]
    public int health;

    [HideInInspector]
    public int xpReward;

    void Awake()
    {
        health = baseHealth;
        xpReward = baseXPReward;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.AddKill();

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            player
                .GetComponent<PlayerXP>()
                .AddXP(xpReward);
        }

        Destroy(gameObject);
    }
}