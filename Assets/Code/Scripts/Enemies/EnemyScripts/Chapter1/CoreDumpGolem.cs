using System.Collections;
using UnityEngine;

public class CoreDumpGolem : Enemy
{
    public float explosionRadius = 3f;
    public float slowDuration = 3f;
    public float slowDebuffFactor = 0.5f;

    void Start()
    {
        hitpoints *= 10; // Very tanky
        moveSpeed /= 2; // Very slow
    }

    public override void TakeDamage(int dmg)
    {
        hitpoints -= dmg;
        if (hitpoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(worth);
            isDestroyed = true;
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Collider2D[] towersInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        foreach (var col in towersInRange)
        {
            BaseTower tower = col.GetComponent<BaseTower>();
            if (tower != null)
            {
                tower.ApplySlow(slowDebuffFactor, slowDuration);
            }
        }

        Debug.Log("Core Dump Golem exploded, causing system slowdown!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
