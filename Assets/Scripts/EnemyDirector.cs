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

    [Header("References")]
    public Transform player;

    private int remainingBudget;

    private float spawnTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        float spawnInterval = Mathf.Max(0.03f, 0.2f - (WaveManager.Instance.currentTier * 0.01f));

        if (spawnTimer >= spawnInterval && remainingBudget > 0)
        {
            spawnTimer = 0f;

            SpawnBudgetEnemy();
        }
    }

    public void StartWave()
    {

        Debug.Log(
            $"Wave {WaveManager.Instance.wave} " +
            $"Budget {remainingBudget}"
        );
        remainingBudget =
            WaveManager.Instance.waveBudget;

        int burst =
            Mathf.RoundToInt(
                WaveManager.Instance.minimumEnemies
                * 0.7f
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

    void ScaleEnemy(
        GameObject enemy)
    {
        int tier =
            WaveManager.Instance.currentTier;

        EnemyHealth health =
            enemy.GetComponent<EnemyHealth>();

        EnemyAI ai =
            enemy.GetComponent<EnemyAI>();

        float statMultiplier =
            Mathf.Pow(
                1.15f,
                tier - 1
            );

        health.health =
            Mathf.RoundToInt(
                health.baseHealth *
                statMultiplier
            );

        health.xpReward =
            Mathf.RoundToInt(
                health.baseXPReward *
                statMultiplier
            );

        ai.speed =
            ai.baseSpeed *
            (1f + (tier * 0.05f));
    }

    Vector3 GetSpawnPosition()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;

        float distance = Random.Range(30f, 40f);

        Vector3 pos = player.position +
            new Vector3(direction.x, 0, direction.y) * distance;

        pos.x = Mathf.Clamp(pos.x, -45f, 145f);
        pos.z = Mathf.Clamp(pos.z, -145f, 45f);

        return pos;
    }

    public int GetRemainingBudget()
    {
        return remainingBudget;
    }
}