using UnityEngine;
using UnityEditor;

public class BuccaneerTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private Transform leftFiringPoint;
    [SerializeField] private Transform rightFiringPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float bulletSpeed = 10f;

    private Transform target;
    private float timeUntilFire;

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / attackSpeed)
            {
                Fire();
                timeUntilFire = 0;
            }
        }
    }

    private void Fire()
    {
        GameObject leftBullet = Instantiate(bulletPrefab, leftFiringPoint.position, Quaternion.identity);
        leftBullet.GetComponent<Rigidbody2D>().linearVelocity = leftFiringPoint.up * bulletSpeed; 

        GameObject rightBullet = Instantiate(bulletPrefab, rightFiringPoint.position, Quaternion.identity);
        rightBullet.GetComponent<Rigidbody2D>().linearVelocity = -leftFiringPoint.up * bulletSpeed; 
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = target.position - leftFiringPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation, 
            Quaternion.Euler(0, 0, angle - 90f), 
            rotationSpeed * Time.deltaTime
        );
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}
