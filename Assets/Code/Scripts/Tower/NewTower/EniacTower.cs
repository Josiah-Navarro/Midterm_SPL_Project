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

    [Header("Visual Effects")]
    [SerializeField] private MeshRenderer towerRenderer;
    private Material towerMaterial;
    private bool isFullyCharged = false;

    protected override void Start()
    {
        if (towerRenderer != null)
        {
            towerMaterial = towerRenderer.material;
        }
        print($"[ENIACTower] Initialized. Attack Range: {towerData.attackRange}");
    }

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
        UpdateVisuals();
    }

    public override void Shoot()
    {
        if (currentTarget == null || towerData.bulletPrefab == null || firingPoint == null)
        {
            print("[ENIACTower] Cannot shoot! Target/BulletPrefab/FiringPoint is null.");
            return;
        }

        float finalDamage = towerData.damage + currentCharge * damageMultiplier;
        if (currentTarget.enemyName == lastEnemyType)
        {
            finalDamage *= 2.5f;
        }

        print($"[ENIACTower] Shooting at {currentTarget.name} for {finalDamage} damage!");

        GameObject bulletObj = Instantiate(towerData.bulletPrefab, firingPoint.position, Quaternion.identity);
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
        print("[ENIACTower] Trying to switch target...");
        FindTarget();
        currentTarget = target ? target.GetComponent<Enemy>() : null;

        if (currentTarget == null)
        {
            print("[ENIACTower] No valid target found. Resetting charge.");
            currentCharge = 0f;
        }
        else
        {
            print($"[ENIACTower] Switched to {currentTarget.name}");
            if (currentTarget.enemyName == lastEnemyType)
            {
                currentCharge *= 1.5f; // Faster recharge if same type
                print($"[ENIACTower] Same enemy type detected! Faster charge: {currentCharge}");
            }
        }
    }

    protected override void FindTarget()
    {
        print($"[ENIACTower] Searching for targets within {towerData.attackRange} range...");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerData.attackRange, enemyMask);
        print($"[ENIACTower] Found {hits.Length} potential targets.");

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
                print($"[ENIACTower] Found valid enemy: {enemy.name}");
                target = enemy.transform;
                return;
            }
        }

        print("[ENIACTower] No valid enemy found in range.");
        target = null;
    }

    protected override void RotateTowardsTarget()
    {
        if (target == null)
        {
            print("[ENIACTower] No target to rotate towards.");
            return;
        }

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        print($"[ENIACTower] Rotating to {angle} degrees towards {target.gameObject.name}");

        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            towerData.rotationSpeed * Time.deltaTime
        );
    }

    public void UpdateVisuals()
    {
        if (towerMaterial != null)
        {
            float glowIntensity = currentCharge / chargeTime;
            towerMaterial.SetFloat("_GlowIntensity", glowIntensity);
        }
    }
}
