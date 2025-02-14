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
    }

    protected override void Update()
    {
        if(target == null)
        {
            FindTarget();   
        }
        if (currentTarget == null || currentTarget.isDestroyed)
        {
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
        if (!isFullyCharged) // Prevent overcharging and multiple shots
        {
            currentCharge += Time.deltaTime;
            if (currentCharge >= chargeTime)
            {
                currentCharge = chargeTime;
                isFullyCharged = true;
                Shoot();
            }
        }
        UpdateVisuals();
    }

    protected override void Shoot()
    {
        if (currentTarget == null || towerData.bulletPrefab == null || firingPoint == null) return;
        
        float finalDamage = towerData.damage + currentCharge * damageMultiplier;
        if (currentTarget.enemyName == lastEnemyType)
        {
            finalDamage = (towerData.damage + currentCharge * damageMultiplier) * 2.5f;
        } 
        // Spawn and initialize bullet
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
        FindTarget();
        currentTarget = target ? target.GetComponent<Enemy>() : null;
        if (currentTarget == null)
        {
            currentCharge = 0f;
        }
        else if (currentTarget.enemyName == lastEnemyType)
        {
            currentCharge *= 1.5f; // Faster recharge if same type
        }
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
