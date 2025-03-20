using System.Collections;
using UnityEngine;

public class MothBoss : Enemy
{
    public GameObject smallBugPrefab;
    public int spawnOnDeathCount = 5;
    public float spawnInterval = 3f;
    public float disableRadius = 3f;
    public float disableDuration = 1f;

    void Start()
    {
        StartCoroutine(SpawnSmallBugs());
    }
    new void Update()
    {
        base.Update();
    }

    IEnumerator SpawnSmallBugs()
    {
        while (hitpoints > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(smallBugPrefab, transform.position, Quaternion.identity);
            DisableNearbyTowers();
        }
    }

    public override void TakeDamage(int dmg)
    {
        hitpoints -= dmg;
        if (hitpoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(worth);
            Die();
        }
    }

    protected void Die()
    {
        for (int i = 0; i < spawnOnDeathCount; i++)
        {
            Instantiate(smallBugPrefab, transform.position, Quaternion.identity);
        }
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void DisableNearbyTowers()
    {
        Collider2D[] towersInRange = Physics2D.OverlapCircleAll(transform.position, disableRadius);
        
        foreach (var col in towersInRange)
        {
            BaseTower tower = col.GetComponent<BaseTower>();
            if (tower != null)
            {
                tower.DisableTower();
                StartCoroutine(ReenableTowerAfterDelay(tower));
            }
        }
        Debug.Log("MothBoss disrupted towers in range!");
    }

    private IEnumerator ReenableTowerAfterDelay(BaseTower tower)
    {
        yield return new WaitForSeconds(disableDuration);
        tower.EnableTower();
    }
}
