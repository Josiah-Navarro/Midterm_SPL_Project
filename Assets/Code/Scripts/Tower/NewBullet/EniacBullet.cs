using UnityEngine;

public class ENIACBullet : BaseBullet
{
    [Header("AoE Attributes")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float aoeDamageRatio = 0.25f;
    [SerializeField] private LayerMask enemyMask;
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color baseColor = Color.blue;
    [SerializeField] private Color chargedColor = Color.cyan;
    [SerializeField] private GameObject explosionEffectPrefab;

    private float chargeProgress = 0f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        chargeProgress += Time.deltaTime / bulletLifetime;
        spriteRenderer.color = Color.Lerp(baseColor, chargedColor, chargeProgress);
    }

    protected override void ApplyDamage()
    {
        if (target == null) return;

        Enemy mainEnemy = target.GetComponent<Enemy>();
        if (mainEnemy != null)
        {
            mainEnemy.TakeDamage(damage);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(target.position, explosionRadius, enemyMask);
        foreach (Collider2D hit in hitEnemies)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy != mainEnemy)
            {
                enemy.TakeDamage((int)(damage * aoeDamageRatio));
            }
        }

        // Trigger explosion visual effect
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
