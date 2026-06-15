using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public static EnemyDirector Instance;

    [Header("Enemy Prefabs")]
    public GameObject gruntPrefab;
    public GameObject runnerPrefab;
    public GameObject tankPrefab;
    public GameObject flyingPrefab;
    public GameObject ghostPrefab;

    [Header("Boss Prefabs")]
    public GameObject slimeBossPrefab;
    public GameObject goblinBossPrefab;
    public GameObject spiderBossPrefab;
    public GameObject monsterBossPrefab;
    public GameObject ghostBossPrefab;

    [Header("References")]
    public Transform player;

    private int remainingBudget;
    private float spawnTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        float spawnInterval =
            Mathf.Max(
                0.03f,
                0.2f - (WaveManager.Instance.currentTier * 0.01f)
            );

        if (spawnTimer >= spawnInterval && remainingBudget > 0)
        {
            spawnTimer = 0f;

            SpawnBudgetEnemy();
        }
    }

    public void StartWave()
    {
        remainingBudget =
            WaveManager.Instance.waveBudget;

        
        if (WaveManager.Instance.wave % 5 == 0)
        {
            SpawnTierBoss();
        }

        int burst =
            Mathf.RoundToInt(
                WaveManager.Instance.minimumEnemies * 0.7f
            );

        for (int i = 0; i < burst; i++)
        {
            SpawnBudgetEnemy();
        }
    }

    void SpawnBudgetEnemy()
    {
        GameObject prefab =
            GetEnemyForBudget();

        if (prefab == null)
            return;

        EnemyHealth data =
            prefab.GetComponent<EnemyHealth>();

        remainingBudget -= data.cost;

        GameObject enemy =
            Instantiate(
                prefab,
                GetSpawnPosition(),
                Quaternion.identity
            );

        ScaleEnemy(enemy);

    }

    GameObject GetEnemyForBudget()
    {
        List<GameObject> possible =
            new List<GameObject>();

        int tier =
            WaveManager.Instance.currentTier;

        EnemyHealth grunt =
            gruntPrefab.GetComponent<EnemyHealth>();

        EnemyHealth runner =
            runnerPrefab.GetComponent<EnemyHealth>();

        EnemyHealth tank =
            tankPrefab.GetComponent<EnemyHealth>();

        EnemyHealth flying =
            flyingPrefab.GetComponent<EnemyHealth>();

        EnemyHealth ghost =
            ghostPrefab.GetComponent<EnemyHealth>();

        if (
            tier >= 1 &&
            grunt.cost <= remainingBudget
        )
        {
            possible.Add(gruntPrefab);
        }

        if (
            tier >= 2 &&
            runner.cost <= remainingBudget
        )
        {
            possible.Add(runnerPrefab);
        }

        if (
            tier >= 3 &&
            tank.cost <= remainingBudget
        )
        {
            possible.Add(tankPrefab);
        }

        if (
            tier >= 4 &&
            flying.cost <= remainingBudget
        )
        {
            possible.Add(flyingPrefab);
        }

        if (
            tier >= 4 &&
            ghost.cost <= remainingBudget
        )
        {
            possible.Add(ghostPrefab);
        }

        if (possible.Count == 0)
            return null;

        return possible[
            Random.Range(
                0,
                possible.Count
            )
        ];
    }

    void ScaleEnemy(GameObject enemy)
    {
        int wave =
            WaveManager.Instance.wave;

        int tier =
            WaveManager.Instance.currentTier;

        EnemyHealth health =
            enemy.GetComponent<EnemyHealth>();

        EnemyAI ai =
            enemy.GetComponent<EnemyAI>();

        float waveMultiplier =
            Mathf.Pow(
                1.08f,
                wave - 1
            );

        float tierMultiplier =
            1f +
            ((tier - 1) * 0.15f);

        float finalMultiplier =
            waveMultiplier *
            tierMultiplier;

        health.health =
            Mathf.RoundToInt(
                health.baseHealth *
                finalMultiplier
            );

        health.xpReward =
            Mathf.RoundToInt(
                health.baseXPReward *
                finalMultiplier
            );

        if (ai != null)
        {
            ai.speed =
                ai.baseSpeed *
                (1f + wave * 0.01f);
        }
        else
        {
            flyingEnemyAi flyingAi =
                enemy.GetComponent<flyingEnemyAi>();

            if (flyingAi != null)
            {
                flyingAi.speed =
                    flyingAi.baseSpeed *
                    (1f + wave * 0.01f);
            }
        }

        float damageMultiplier =
    Mathf.Pow(
        1.04f,
        wave - 1
    );

        ai.damage =
            Mathf.RoundToInt(
                ai.baseDamage *
                damageMultiplier
            );
    }

    void SpawnTierBoss()
    {
        GameObject bossPrefab = null;

        switch (WaveManager.Instance.wave)
        {
            case 5:
                bossPrefab = slimeBossPrefab;
                break;

            case 10:
                bossPrefab = goblinBossPrefab;
                break;

            case 15:
                bossPrefab = spiderBossPrefab;
                break;

            case 20:
                bossPrefab = monsterBossPrefab;
                break;

            case 25:
                bossPrefab = ghostBossPrefab;
                break;

            default:

                List<GameObject> bosses =
                    new List<GameObject>();

                if (slimeBossPrefab != null)
                    bosses.Add(slimeBossPrefab);

                if (goblinBossPrefab != null)
                    bosses.Add(goblinBossPrefab);

                if (spiderBossPrefab != null)
                    bosses.Add(spiderBossPrefab);

                if (monsterBossPrefab != null)
                    bosses.Add(monsterBossPrefab);

                if (ghostBossPrefab != null)
                    bosses.Add(ghostBossPrefab);

                if (bosses.Count == 0)
                    return;

                bossPrefab =
                    bosses[
                        Random.Range(
                            0,
                            bosses.Count
                        )
                    ];

                break;
        }

        GameObject boss =
            Instantiate(
                bossPrefab,
                GetSpawnPosition(),
                Quaternion.identity
            );

        ScaleBoss(boss);

        BossAbility ability =
boss.AddComponent<BossAbility>();

        ability.isFlyingBoss =
            boss.GetComponent<flyingEnemyAi>() != null;

    }

    void ScaleBoss(GameObject boss)
    {
        int wave =
            WaveManager.Instance.wave;

        int tier =
            WaveManager.Instance.currentTier;

        EnemyHealth health =
            boss.GetComponent<EnemyHealth>();

        EnemyAI ai =
            boss.GetComponent<EnemyAI>();

        float waveMultiplier =
            Mathf.Pow(
                1.12f,
                wave - 1
            );

        float tierMultiplier =
            2f +
            ((tier - 1) * 0.5f);

        float finalMultiplier =
            waveMultiplier *
            tierMultiplier;

        health.health =
            Mathf.RoundToInt(
                health.baseHealth *
                finalMultiplier
            );

        health.xpReward =
            Mathf.RoundToInt(
                health.baseXPReward *
                finalMultiplier *
                3f
            );

        if (ai != null)
        {
            ai.speed =
                ai.baseSpeed *
                (1f + wave * 0.015f);
        }
        else
        {
            flyingEnemyAi flyingAi =
                boss.GetComponent<flyingEnemyAi>();

            if (flyingAi != null)
            {
                flyingAi.speed =
                    flyingAi.baseSpeed *
                    (1f + wave * 0.015f);
            }
        }

        float damageMultiplier =
    Mathf.Pow(
        1.05f,
        wave - 1
    );

        ai.damage =
            Mathf.RoundToInt(
                ai.baseDamage *
                damageMultiplier *
                2f
            );

        boss.transform.localScale *= 2f;


    }

    Vector3 GetSpawnPosition()
    {
        Vector2 direction =
            Random.insideUnitCircle.normalized;

        float distance =
            Random.Range(
                30f,
                40f
            );

        Vector3 pos =
            player.position +
            new Vector3(
                direction.x,
                0,
                direction.y
            ) * distance;

        pos.x =
            Mathf.Clamp(
                pos.x,
                -45f,
                145f
            );

        pos.z =
            Mathf.Clamp(
                pos.z,
                -145f,
                45f
            );

        return pos;
    }

    public int GetRemainingBudget()
    {
        return remainingBudget;
    }
}