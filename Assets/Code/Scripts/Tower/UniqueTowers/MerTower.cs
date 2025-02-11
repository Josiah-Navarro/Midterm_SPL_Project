using UnityEngine;
using UnityEditor;

public class MerMonkey : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform turretRotationPoint;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1000f;
    private float timeUntilFire;
    private Transform target;

    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / attackSpeed)
        {
            Fire();
            timeUntilFire = 0;
        }
    }

    private void Fire()
    {
        Debug.Log("Echo Bubble Fired!");  // Check if firing happens

        GameObject bubble = Instantiate(bubblePrefab, firingPoint.position, Quaternion.identity);
        EchoBubble bubbleScript = bubble.GetComponent<EchoBubble>();

        if (bubbleScript != null)
            {
            bubbleScript.SetTarget(target);
            Debug.Log("Target assigned: " + target.name);
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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