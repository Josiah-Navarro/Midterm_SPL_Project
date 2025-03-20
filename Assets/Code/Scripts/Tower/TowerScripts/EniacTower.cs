using UnityEngine;

public class ENIACTower : BaseTower
{
    [Header("ENIAC Settings")]
    public float chargeTime = 3f; // Max charge time
    public float damageMultiplier = 10f; // Damage scales with charge time
    public float aoeRatio = 0.25f; // 25% of main damage is AoE
    private float currentCharge = 0f;
    private Enemy currentTarget;
    private string lastEnemyType;

    private bool isFullyCharged = false;

    protected override void Update()
    {
        if (target == null)
        {
            print("[ENIACTower] No target, finding a new one...");
            FindTarget();
        }
        else
        {
            print($"[ENIACTower] Current target: {target.name}");
        }

        if (currentTarget == null || currentTarget.isDestroyed)
        {
            print("[ENIACTower] Switching target...");
            SwitchTarget();
        }

        RotateTowardsTarget();
        if (currentTarget != null)
        {
            ChargeUp();
        }
    }

    private void ChargeUp()
    {
        if (!isFullyCharged)
        {
            currentCharge += Time.deltaTime;
            print($"[ENIACTower] Charging up: {currentCharge}/{chargeTime}");

            if (currentCharge >= chargeTime)
            {
                currentCharge = chargeTime;
                isFullyCharged = true;
                print("[ENIACTower] Fully charged! Shooting...");
                Shoot();
            }
        }
    }

    public override void Shoot()
    {
        if (currentTarget == null || bulletPrefab == null || firingPoint == null)
        {
            return;
        }

        float finalDamage = damage + currentCharge * damageMultiplier;
        if (currentTarget.enemyName == lastEnemyType)
        {
            finalDamage *= 2.5f;
        }


        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        ENIACBullet bullet = bulletObj.GetComponent<ENIACBullet>();
        if (bullet != null)
        {
            bullet.Initialize(currentTarget.transform, (int)finalDamage);
        }

        lastEnemyType = currentTarget.enemyName;
        isFullyCharged = false;
        currentCharge = 0;
    }

    private void SwitchTarget()
    {
        FindTarget();
        currentTarget = target ? target.GetComponent<Enemy>() : null;

        if (currentTarget == null)
        {
            currentCharge = 0f;
        }
        else
        {
            if (currentTarget.enemyName == lastEnemyType)
            {
                currentCharge *= 1.5f; 
            }
        }
    }

    protected override void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentAttackRange, enemyMask);
        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                target = enemy.transform;
                return;
            }
        }

        target = null;
    }

    protected override void RotateTowardsTarget()
    {
        if (target == null)
        {
            return;
        }

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));


        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    
}
