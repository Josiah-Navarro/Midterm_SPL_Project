using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseTower : MonoBehaviour
{
    public enum TargetingMode
    {
        First,
        Last,
        Strong,
        Weak
    }

    [Header("Tower Data")]
    [SerializeField] public TowerData towerData;

    [Header("References")]
    [SerializeField] protected Collider2D collider2D;
    [SerializeField] protected Transform turretRotationPoint;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected Transform firingPoint;

    protected Transform target;
    protected float timeUntilFire;
    protected TargetingMode targetingMode = TargetingMode.First;
    
    private float originalAttackSpeed;
    private float originalAttackRange;
    private float attackRange;
    private float attackSpeed;

    protected virtual void Start()
    {
        if (towerData == null)
        {
            Debug.LogError($"{gameObject.name} is missing TowerData!");
            return;
        }
        
        originalAttackSpeed = towerData.attackSpeed;
        originalAttackRange = towerData.attackRange;
        attackRange = towerData.attackRange;
        attackSpeed = towerData.attackSpeed;
    }

    protected virtual void Update()
    {
        if (target == null || !CheckTargetIsInRange())
        {
            FindTarget();
        }

        if (target != null)
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / attackSpeed)
            {
                // Debug.Log("Shoot");
                Shoot();
                timeUntilFire = 0;
            }
        }
    }

    public virtual void Shoot()
    {
        if (target == null) return;
        GameObject bullet = Instantiate(towerData.bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target, towerData.damage);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing BaseBullet component!");
        }
    }

    protected bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= attackRange;
    }

    protected virtual void RotateTowardsTarget()
    {
        if (target == null) return;

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            towerData.rotationSpeed * Time.deltaTime
        );
    }

    protected virtual void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyMask);
        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        List<Enemy> enemies = new List<Enemy>();
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            target = null;
            return;
        }

        switch (targetingMode)
        {
            case TargetingMode.First:
                target = enemies.OrderBy(e => e.GetDistanceToEnd()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Last:
                target = enemies.OrderByDescending(e => e.GetDistanceToEnd()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Strong:
                target = enemies.OrderByDescending(e => e.GetPlayerDamage()).FirstOrDefault()?.transform;
                break;
            case TargetingMode.Weak:
                target = enemies.OrderBy(e => e.GetPlayerDamage()).FirstOrDefault()?.transform;
                break;
        }
    }

    public float GetAttackRange(){return attackRange;}
    public float GetAttackSpeed(){return attackSpeed;}
    public void SetAttackRange(float aR){attackRange = aR;}
    public void SetAttackSpeed(float aS){attackSpeed = aS;}
    public void Reset()
    {
        attackRange = originalAttackRange;
        attackSpeed = originalAttackSpeed;
    }
#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        if (towerData != null)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position, transform.forward, towerData.attackRange);
        }
    }
#endif
}
