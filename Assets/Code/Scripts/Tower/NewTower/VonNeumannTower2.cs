using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class VonNeumannTower : TestTower
{
    [Header("Attack Settings")]
    public float scalingFactor = 0.02f; // Growth rate per attack
    public GameObject storedProgramBlastPrefab;

    [Header("Buff Settings")]
    public float buffRadius = 6f;
    public float memoryPoolDuration = 5f;
    public float memoryStoredPercentage = 0.1f;
    private float memoryPool;

    [Header("Ultimate Ability")]
    public float replicationDuration = 5f;
    private bool isReplicating = false;

    private List<BaseTower> nearbyTowers = new List<BaseTower>();

    private float currentAttackSpeed;
    private float currentAttackRange;

    protected override void Start()
    {
        base.Start();
        currentAttackSpeed = towerData.attackSpeed;
        currentAttackRange = towerData.attackRange;
        skeletonAnimation.AnimationState.Event += OnAttackEvent;
        StartCoroutine(BuffNearbyTowers());
}
    protected override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        if (target == null) return;
        PlayAnimation("Attack_Begin", false);
    }

    private void OnAttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "OnAttack")
        {
            GameObject blast = Instantiate(storedProgramBlastPrefab, firingPoint.position, Quaternion.identity);
            StoredProgramBlast spb = blast.GetComponent<StoredProgramBlast>();
            if (spb != null)
            {
                spb.Initialize(target, towerData.damage);
            }
            else
            {
                Debug.LogError("StoredProgramBlast component is missing!");
            }

            // Apply Scaling Without Modifying TowerData
            currentAttackSpeed *= (1 + scalingFactor);
            currentAttackRange *= (1 + scalingFactor / 2);
            PlayAnimation("Attack_End", false);
        }
    }

    private IEnumerator BuffNearbyTowers()
    {
        while (true)
        {
            nearbyTowers.Clear();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buffRadius);
            foreach (Collider2D col in colliders)
            {
                BaseTower tower = col.GetComponent<BaseTower>();
                if (tower != null && tower != this)
                {
                    nearbyTowers.Add(tower);
                    memoryPool += tower.towerData.damage * memoryStoredPercentage;
                }
            }
            yield return new WaitForSeconds(memoryPoolDuration);
            ReleaseMemoryPool();
        }
    }

    private void ReleaseMemoryPool()
    {
        foreach (BaseTower tower in nearbyTowers)
        {
            tower.towerData.attackSpeed *= 1.2f;
            tower.towerData.attackRange *= 1.1f;
        }
        memoryPool = 0;
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
            tower.Shoot();
        }
    }
}
