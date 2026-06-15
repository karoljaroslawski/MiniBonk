using System.Collections;
using UnityEngine;

public class BossAbility : MonoBehaviour
{
    public bool isFlyingBoss;

    EnemyAI enemyAI;
    flyingEnemyAi flyingAI;

    float baseSpeed;
    float baseShootCooldown;

    Transform player;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        flyingAI = GetComponent<flyingEnemyAi>();

        if (enemyAI != null)
            baseSpeed = enemyAI.speed;

        if (flyingAI != null)
            baseShootCooldown = flyingAI.currentShootCooldown;

        player =
            GameObject.Find("Player").transform;

        StartCoroutine(AbilityLoop());
    }

    IEnumerator AbilityLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (isFlyingBoss)
            {
                yield return StartCoroutine(FireRateBoost());
            }
            else
            {
                yield return StartCoroutine(SpeedBoost());

            }
        }
    }

    IEnumerator SpeedBoost()
    {
        if (enemyAI == null)
            yield break;

        enemyAI.speed = baseSpeed * 2f;

        yield return new WaitForSeconds(2f);

        enemyAI.speed = baseSpeed;
    }



    IEnumerator FireRateBoost()
    {
        if (flyingAI == null)
            yield break;

        flyingAI.currentShootCooldown =
            baseShootCooldown * 0.5f;

        yield return new WaitForSeconds(1f);

        flyingAI.currentShootCooldown =
            baseShootCooldown;
    }
}