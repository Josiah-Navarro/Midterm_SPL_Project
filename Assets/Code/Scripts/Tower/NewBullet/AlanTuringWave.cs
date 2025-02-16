using UnityEngine;

public class AlanTuringWave : BaseWave
{
    [Header("Attributes")]
    [SerializeField] public float conversionChance = 0.1f;
    [SerializeField] public int decipherDamage = 10;
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
                TryConvertEnemy(enemy);
            }
        }
    }
    void TryConvertEnemy(Enemy enemy)
    {
        if (Random.value <= conversionChance && enemy.CanBeConverted)
        {
            ConvertToAlly(enemy);
        } else{
            Debug.Log("Coversion Fail");
        }
    }

    public void ConvertToAlly(Enemy enemy)
    {
        if (!enemy.CanBeConverted) return;
        GameObject ally = Instantiate(allyPrefab, enemy.transform.position, Quaternion.identity);
        Ally allyObj  = ally.GetComponent<Ally>();
        allyObj.Initialize(enemy.moveSpeed, enemy.hitpoints);
        enemy.DestroyMe();
    }
    
}