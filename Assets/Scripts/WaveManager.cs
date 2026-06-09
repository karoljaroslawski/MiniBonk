using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave")]
    public int wave = 1;

    [Header("Tier")]
    public int currentTier = 1;

    [Header("Budget")]
    public int waveBudget;

    [Header("Enemy Count")]
    public int minimumEnemies;

    [Header("Wave Timer")]
    public float waveDuration = 30f;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CalculateWave();

        UIManager.Instance.UpdateWave(
            wave
        );

        EnemyDirector.Instance.StartWave();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float remainingTime =
            waveDuration - timer;

        UIManager.Instance.UpdateWaveTimer(
            remainingTime
        );

        if (timer >= waveDuration)
        {
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        timer = 0f;

        wave++;

        CalculateWave();

        UIManager.Instance.UpdateWave(
            wave
        );

        EnemyDirector.Instance.StartWave();
    }

    void CalculateWave()
    {
        currentTier =
            ((wave - 1) / 5) + 1;

        int budgetGrowth =
            20 +
            ((currentTier - 1) * 10);

        waveBudget =
            wave *
            budgetGrowth;

        minimumEnemies =
            5 + wave;

        Debug.Log(
            $"Wave {wave} | " +
            $"Tier {currentTier} | " +
            $"Budget {waveBudget}"
        );
    }
}