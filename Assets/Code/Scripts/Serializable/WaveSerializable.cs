using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;  // The enemy type to spawn
    public int count;               // How many enemies of this type
    public float spawnDelay;        // Delay between spawns
}

[CreateAssetMenu(fileName = "NewWave", menuName = "Tower Defense/Wave")]
public class WaveSerializable : ScriptableObject
{
    public EnemySpawnInfo[] enemies; // List of enemy types and their counts
    public float waveDelay = 5f;     // Delay before next wave starts
}
