using UnityEngine;

public class PilotTower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 300f;

    [Header("Firing Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform frontFirePoint;
    [SerializeField] private Transform backFirePoint;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float targetingRange = 8f;
    [SerializeField] private float fireRate = 2f;

    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    private Transform target;
    private float fireCooldown = 0f;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        ChaseTarget();
        RotateTowardsTarget();
        
        fireCooldown += Time.deltaTime;
        if (fireCooldown >= 1f / fireRate)
        {
            Shoot();
            fireCooldown = 0;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
    }

    private void FindTarget()
    {
        if (target != null) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void ChaseTarget()
    {
        if (target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (target == null) return;
        
        Transform[] firePoints = { frontFirePoint, backFirePoint, leftFirePoint, rightFirePoint };
        foreach (Transform firePoint in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(target);
            }
        }
    }

}
