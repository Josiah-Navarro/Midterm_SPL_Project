using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredProgramBlast : BaseBullet
{
    public float explosionRadius = 3f;
    public float explosionDelay = 1.5f;
    public LayerMask enemyLayer;

    private Vector2 targetPosition;

    public override void Initialize(Transform newTarget, int newDamage)
    {
        base.Initialize(newTarget, newDamage);
        targetPosition = newTarget.position;
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(MoveTowardsTarget());
    }

    private IEnumerator MoveTowardsTarget()
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, bulletSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(Detonate());
    }

    private IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explosionDelay);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(targetPosition, explosionRadius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetPosition, explosionRadius);
    }
}
