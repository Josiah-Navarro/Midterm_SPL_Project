using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VonNeumannTower : BaseTower
{
    [Header("Attack Settings")]
    public float scalingFactor = 0.02f; // Growth rate per attack
    public GameObject storedProgramBlastPrefab;

    [Header("Buff Settings")]
    public float buffRadius = 6f;
    public float memoryPoolDuration = 5f;
    public float memoryStoredPercentage = 0.1f;
    private float memoryPool;
    public LayerMask towerMask;

    [Header("Ultimate Ability")]
    public float replicationDuration = 5f;
    private bool isReplicating = false;

    private List<BaseTower> nearbyTowers = new List<BaseTower>();

    private float currentAttackSpeed;
    private float currentAttackRange;

    protected override void Start()
    {
        base.Start();
        currentAttackSpeed = towerData.attackSpeed; // Store initial attack speed without modifying towerData
        currentAttackRange = towerData.attackRange; // Store initial attack range

        StartCoroutine(BuffNearbyTowers());

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundStart.AddListener(ResetScaling);
        }
    }

    protected override void Update()
    {
        if (target == null || !CheckTargetIsInRange())
        {
            FindTarget();
        }

        if (target != null)
        {
            RotateTowardsTarget();
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / currentAttackSpeed)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }
    }

    private void OnDestroy()
    {
        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundStart.RemoveListener(ResetScaling);
        }
    }

    public override void Shoot()
    {
        if (target == null) return;
        GameObject blast = Instantiate(storedProgramBlastPrefab, firingPoint.position, Quaternion.identity);
        StoredProgramBlast spb = blast.GetComponent<StoredProgramBlast>();
        spb.Initialize(target, towerData.damage);

        // Apply Scaling Without Modifying TowerData
        currentAttackSpeed *= (1 + scalingFactor);
        currentAttackRange *= (1 + scalingFactor / 2);

    }   
    private float memoryCount;

    private IEnumerator BuffNearbyTowers()
    {   
        while (true)
        {
            nearbyTowers.Clear();
            memoryPool = 0; // Reset memory pool at the start of each cycle

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buffRadius);
            foreach (Collider2D col in colliders)
            {
                BaseTower tower = col.GetComponent<BaseTower>();
                if (tower != null && tower != this)
                {                    
                    nearbyTowers.Add(tower);
                    memoryPool += tower.towerData.damage * memoryStoredPercentage;
                    Debug.Log($"[VonNeumannTower] Found nearby tower: {tower.gameObject.name}, Memory Pool: {memoryPool}");
                }
            }

            yield return new WaitForSeconds(memoryPoolDuration);
            ReleaseMemoryPool();
        }
    }

    private void ReleaseMemoryPool()
    {
        if (nearbyTowers.Count > 0)
        {
            Debug.Log($"[VonNeumannTower] Buffing {nearbyTowers.Count} towers. Applying memory pool effect...");
        }
        else
        {
            Debug.Log("[VonNeumannTower] No towers to buff this cycle.");
        }

        foreach (BaseTower tower in nearbyTowers)
        {
            float newAttackSpeed = tower.GetAttackSpeed() * 1.1f;
            float newAttackRange = tower.GetAttackRange() * 1.1f;

            tower.SetAttackSpeed(newAttackSpeed);
            tower.SetAttackRange(newAttackRange);

            Debug.Log($"[VonNeumannTower] Buffed {tower.gameObject.name}: New Attack Speed = {newAttackSpeed}, New Attack Range = {newAttackRange}");
        }

        memoryPool = 0; // Reset memory pool after application
    }


    public void ActivateReplicationSurge()
    {
        if (!isReplicating)
        {
            StartCoroutine(ReplicationSurge());
        }
    }

    private IEnumerator ReplicationSurge()
    {
        isReplicating = true;
        foreach (BaseTower tower in nearbyTowers)
        {
            StartCoroutine(DuplicateAttacks(tower));
        }
        yield return new WaitForSeconds(replicationDuration);
        isReplicating = false;
    }

    private IEnumerator DuplicateAttacks(BaseTower tower)
    {
        while (isReplicating)
        {
            yield return new WaitForSeconds(tower.towerData.attackSpeed);
            tower.Shoot(); // Simulating an extra attack
        }
    }

    private void ResetScaling()
    {
        Debug.Log("ResetScaling called for VonNeumannTower!");
        currentAttackSpeed = towerData.attackSpeed; // Reset to original attack speed
        currentAttackRange = towerData.attackRange; // Reset to original attack range
    }
#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        if (towerData != null)
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, transform.forward, buffRadius);

        }
    }
#endif
}