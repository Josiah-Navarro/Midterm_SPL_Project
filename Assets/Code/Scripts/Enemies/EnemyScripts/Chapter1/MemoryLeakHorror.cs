using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryLeakHorror : Enemy
{
    [Header("Memory Leak Properties")]
    public float hpIncreaseRate = 5f; 
    public GameObject memoryFragmentPrefab;
    public float fragmentSpawnInterval = 3f;
    
    private float fragmentSpawnTimer = 0f;

    new void Update()
    {
        base.Update();

        fragmentSpawnTimer += Time.deltaTime;
        if (fragmentSpawnTimer >= fragmentSpawnInterval)
        {
            hitpoints += 1;
            SpawnMemoryFragment();
            fragmentSpawnTimer = 0f;
        }
    }

    void SpawnMemoryFragment()
    {
        if (memoryFragmentPrefab != null)
        {
            int randomPathIndex = Random.Range(0, LevelManager.main.paths.Count);
            List<Transform> selectedPath = LevelManager.main.paths[randomPathIndex].waypoints;

            GameObject bugFragment = Instantiate(memoryFragmentPrefab, selectedPath[0].position, Quaternion.identity);
            Enemy bug = bugFragment.GetComponent<Enemy>();
            bug.Initialize(randomPathIndex);
            
        }
    }
}
