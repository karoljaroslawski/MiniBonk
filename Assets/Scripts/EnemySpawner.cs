using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnTime = 2f;

    void Start()
    {

    }

    void SpawnEnemy()
    {
        Vector3 randomPosition =
            new Vector3(
                Random.Range(-20, 20),
                1,
                Random.Range(-20, 20)
            );

        GameObject enemy =
            Instantiate(
                enemyPrefab,
                randomPosition,
                Quaternion.identity
            );

        EnemyHealth health =
            enemy.GetComponent<EnemyHealth>();

        EnemyAI ai =
            enemy.GetComponent<EnemyAI>();


    }
}