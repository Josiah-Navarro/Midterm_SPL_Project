using UnityEngine;

public class AlanTuringWave : BaseWave
{
    [Header("Attributes")]
    [SerializeField] private float conversionChance = 0.3f;
    [SerializeField] private int decipherDamage = 10;

    [Header("Ally Spawn")]
    [SerializeField] private GameObject allyPrefab;

    public override void TriggerEffect()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, maxRadius, enemyLayer);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            Enemy enemy = enemyCol.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(decipherDamage);
                
                if (enemy.hitpoints > 0)
                {
                    TryConvertEnemy(enemy);
                }
            }
        }
    }

    private void TryConvertEnemy(Enemy enemy)
    {
        if (enemy.CanBeConverted && Random.value <= conversionChance)
        {
            ConvertToAlly(enemy);
        }
        else
        {
            Debug.Log($"Conversion Failed: {enemy.enemyName} remains an enemy.");
        }
    }

    private void ConvertToAlly(Enemy enemy)
    {
        if (!enemy.CanBeConverted) return;

        GameObject ally = Instantiate(allyPrefab, enemy.transform.position, Quaternion.identity);
        Ally allyObj = ally.GetComponent<Ally>();
        if (allyObj != null)
        {
            allyObj.Initialize(enemy.GetMoveSpeed(), enemy.originalHitpoints, enemy.pathIndex);
        }

        enemy.DestroyMe();
        Debug.Log($"{enemy.enemyName} successfully converted into an ally!");
    }
}
