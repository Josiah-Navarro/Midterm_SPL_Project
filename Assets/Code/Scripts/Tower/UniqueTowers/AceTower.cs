using UnityEngine;
using UnityEditor;
public class AcePilot : MonoBehaviour
{   
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float flightRadius = 5f;
    [SerializeField] private float rotationSpeed = 500f;


    private Vector3 orbitCenter; // The clicked plot position
    private float angle = 0f;
    
    [Header("Firing Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float targetingRange = 5f;

    [SerializeField] private float fireRate = 1f;
    
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform leftRotatingPoint;
    [SerializeField] private Transform rightRotatingPoint;

    private float fireCooldown = 1f;
    private bool useLeftFirePoint = true;
    private Transform target;


    private void Start()
    {
        // Get the starting position from the plot automatically
        orbitCenter = transform.position;
        transform.position = orbitCenter + new Vector3(flightRadius, 0, 0); // Start to the right
    }

    private void Update()
    {
        MoveInCircle();
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTurretTowardsTarget();
        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            fireCooldown += Time.deltaTime;
            if (fireCooldown >= 1f / fireRate)
            {
                Shoot();
                fireCooldown = 0;
            }
        }
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTurretTowardsTarget()
    {
        if (target == null) return;

        Transform rotatingPoint = useLeftFirePoint ? leftRotatingPoint : rightRotatingPoint;

        Vector3 direction = target.position - rotatingPoint.position;   
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotatingPoint.rotation = Quaternion.RotateTowards(
            rotatingPoint.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime
        );
    }


    private void FindTarget()
    {
        if (target != null) return;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);
        if (hits.Length > 0)
        {
            Debug.Log("Target Found: ");
            target = hits[0].transform;
        }
    }
    private void MoveInCircle()
    {
        angle += moveSpeed * Time.deltaTime;

        float x = Mathf.Cos(angle) * flightRadius;
        float y = Mathf.Sin(angle) * flightRadius;

        transform.position = orbitCenter + new Vector3(x, y, 0);

        Vector3 direction = transform.position - orbitCenter;
        float angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }

    private void Shoot()
    {
        Transform firePoint = useLeftFirePoint ? leftFirePoint : rightFirePoint;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Homing bulletScript = bullet.GetComponent<Homing>();
        bulletScript.SetTarget(target);
        useLeftFirePoint = !useLeftFirePoint;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}
