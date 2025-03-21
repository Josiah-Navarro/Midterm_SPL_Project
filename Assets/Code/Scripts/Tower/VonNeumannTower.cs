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

    private float CAS;
    private float CAR;

    protected override void Start()
    {
        base.Start();
        CAS = attackSpeed; // Store initial attack speed without modifying towerData
        CAR = attackRange; // Store initial attack range

        StartCoroutine(BuffNearbyTowers());

        if (RoundManager.Instance != null)
        {
            RoundManager.Instance.OnRoundStart.AddListener(ResetScaling);
        }
    }
    new void Update()
    {
        if(isDisabled){
            StopAllCoroutines();
            return;
        }
        base.Update();  
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
        spb.Initialize(target, damage);

        CAS *= (1 + scalingFactor);
        CAR *= (1 + scalingFactor / 2);

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
                    memoryPool += tower.damage * memoryStoredPercentage;
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
            float newAttackSpeed = tower.GetAttackSpeed() * 1.1f;
            float newAttackRange = tower.GetAttackRange() * 1.1f;

            tower.SetAttackSpeed(newAttackSpeed);
            tower.SetAttackRange(newAttackRange);

            Debug.Log($"[VonNeumannTower] Buffed {tower.gameObject.name}: New Attack Speed = {newAttackSpeed}, New Attack Range = {newAttackRange}");
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
            yield return new WaitForSeconds(tower.attackSpeed);
            tower.Shoot(); 
        }
    }

    private void ResetScaling()
    {
        CAS = attackSpeed; // Reset to original attack speed
        CAR = attackRange; // Reset to original attack range
        foreach (BaseTower tower in nearbyTowers)
        {
            float newAttackSpeed = tower.GetAttackSpeed() * 1.1f;
            float newAttackRange = tower.GetAttackRange() * 1.1f;

            tower.Reset();

        }
    }
#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, buffRadius);
    }
#endif
}