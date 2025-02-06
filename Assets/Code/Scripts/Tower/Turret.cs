using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f; // Range duh
    [SerializeField] private float rotationSpeed = 10f; //Rotation Speed duh
    [SerializeField] private float attackSpeed = 1f; //Attack SPeed
    private Transform target;
    private float timeUntilFire; //Reload

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();
        if (!CheckTargetIsInRage()){
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f/attackSpeed)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }
    }
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private bool CheckTargetIsInRage()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Corrected the typo here as well
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
