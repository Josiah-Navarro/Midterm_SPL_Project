using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private EnemyManager[] enemies; 

    [Header("Attributes")]
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    public void InitiateWave(bool _start)
    {
        if (_start)
        {
            StartCoroutine(StartWave());
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        enemiesLeftToSpawn = EnemiesPerWave();
        isSpawning = true;
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
    }

    private int EnemiesPerWave()
    {
        int totalEnemies = 0;
        foreach (EnemyManager enemy in enemies)
        {
            totalEnemies += Mathf.RoundToInt(enemy.spawnCount * Mathf.Pow(currentWave, difficultyScalingFactor));
        }
        Debug.Log("Calculated enemies for this wave: " + totalEnemies);
        return totalEnemies;
    }

    private void SpawnEnemy()
    {
        if (enemies.Length == 0) return;
        GameObject prefabToSpawn = ChooseRandomEnemy();
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private GameObject ChooseRandomEnemy()
    {
        int totalSpawnWeight = 0;
        foreach (EnemyManager enemy in enemies)
        {
            totalSpawnWeight += enemy.spawnCount;
        }

        int randomWeight = Random.Range(0, totalSpawnWeight);
        foreach (EnemyManager enemy in enemies)
        {
            if (randomWeight < enemy.spawnCount)
                return enemy.prefab;
            randomWeight -= enemy.spawnCount;
        }

        return enemies[0].prefab;
    }
}
