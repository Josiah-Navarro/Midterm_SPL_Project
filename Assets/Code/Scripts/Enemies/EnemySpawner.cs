using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private WaveSerializable[] waves; // Array of waves
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private int currentWaveIndex = 0;

    private bool isSpawning = false;
    private bool roundActive = false;
    private List<EnemySpawnData> spawnQueue = new List<EnemySpawnData>(); // Track enemies per wave

    public static EnemySpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isSpawning || !roundActive || spawnQueue.Count == 0) return;

        for (int i = spawnQueue.Count - 1; i >= 0; i--)
        {
            EnemySpawnData spawnData = spawnQueue[i];

            spawnData.timeSinceLastSpawn += Time.deltaTime;

            if (spawnData.timeSinceLastSpawn >= spawnData.spawnDelay && spawnData.remainingCount > 0)
            {
                // Get a random path from LevelManager
                int randomPathIndex = Random.Range(0, LevelManager.main.paths.Count); // Random index from available paths
                List<Transform> selectedPath = LevelManager.main.paths[randomPathIndex].waypoints;

                // Spawn enemy
                GameObject enemy = Instantiate(spawnData.enemyPrefab, selectedPath[0].position, Quaternion.identity);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Initialize(randomPathIndex); // Call the initialization function
                }

                spawnData.remainingCount--;
                spawnData.timeSinceLastSpawn = 0f;
            }

            if (spawnData.remainingCount <= 0)
            {
                spawnQueue.RemoveAt(i);
            }
        }

        // If all enemies are spawned and no more in the queue, stop spawning
        if (spawnQueue.Count == 0)
        {
            StopSpawning();
        }
    }

    public void StartRound()
    {
        if (currentWaveIndex >= waves.Length) return;

        roundActive = true;
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(waves[currentWaveIndex].waveDelay);
        isSpawning = true;

        spawnQueue.Clear();
        foreach (EnemySpawnInfo enemy in waves[currentWaveIndex].enemies)
        {
            spawnQueue.Add(new EnemySpawnData(enemy.enemyPrefab, enemy.count, enemy.spawnDelay));
        }

        currentWaveIndex++;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        roundActive = false;
        spawnQueue.Clear();
    }
    private void EnemyDestroyed(){}
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int remainingCount;
    public float spawnDelay;
    public float timeSinceLastSpawn;

    public EnemySpawnData(GameObject prefab, int count, float delay)
    {
        enemyPrefab = prefab;
        remainingCount = count;
        spawnDelay = delay;
        timeSinceLastSpawn = 0f;
    }
}
