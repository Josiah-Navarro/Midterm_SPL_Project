using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private EnemySerializable[] enemies; 

    [Header("Attributes")]
    [SerializeField] private float baseEnemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.1f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private bool isSpawning = false;
    private bool roundActive = false;
    public static EnemySpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        onEnemyDestroy.AddListener(EnemyDestroyed);

        // Subscribe to round end
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnd.AddListener(StopSpawning);
        }
    }

    private void OnDestroy()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundEnd.AddListener(StopSpawning);
        }
    }

    void Update()
    {
        if (!isSpawning || !roundActive) return;

        timeSinceLastSpawn += Time.deltaTime;

        float adjustedSpawnRate = baseEnemiesPerSecond * (1 + (currentWave - 1) * difficultyScalingFactor);
        if (timeSinceLastSpawn >= (1f / adjustedSpawnRate))
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }
    }

    private void EnemyDestroyed()
    {
        // This function is no longer used to determine wave end
    }

    public void StartRound()
    {
        roundActive = true;
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        roundActive = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
    }

    private void SpawnEnemy()
    {
        if (enemies.Length == 0 || !roundActive) return;
        GameObject prefabToSpawn = ChooseRandomEnemy();
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private GameObject ChooseRandomEnemy()
    {
        if (enemies.Length == 0) return null;

        float totalWeight = 0f;
        foreach (EnemySerializable enemy in enemies)
        {
            totalWeight += enemy.spawnCount; // spawnCount is now the probability weight
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (EnemySerializable enemy in enemies)
        {
            cumulativeWeight += enemy.spawnCount;
            if (randomValue <= cumulativeWeight)
            {
                return enemy.prefab;
            }
        }

        return enemies[0].prefab; // Fallback in case something goes wrong
    }
}
