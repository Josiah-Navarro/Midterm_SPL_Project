// using UnityEngine;
// using System.Collections.Generic;

// public class SubTower : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private LayerMask enemyMask;
//     [SerializeField] private Transform rotatingPoint;
//     [SerializeField] private Transform firingPoint;
//     [SerializeField] private GameObject bulletPrefab;

//     [Header("Attributes")]
//     [SerializeField] private float targetingRange;
//     [SerializeField] private float aps = 1f; // Attacks per second
//     [SerializeField] private float rotatingSpeed = 500f;

//     private Transform target;
//     private float timeUntilFire;
//     private List<SubTower> allSubs; // Only stores Sub Towers

//     void Awake()
//     {
//         timeUntilFire = 1f / aps;
//         FindAllSubTowers();
//     }

//     void Update()
//     {
//         FindTarget(); // Locate the nearest enemy
//         RotateToTarget(); // Rotate toward the enemy
//         FireAtTarget(); // Attack logic
//     }

//     void FindAllSubTowers()
//     {
//         // Get all Sub Towers in the scene using the new method
//         SubTower[] subs = FindObjectsByType<SubTower>(FindObjectsSortMode.None);
//         allSubs = new List<SubTower>(subs);
//     }


//     void FindTarget()
//     {
//         List<Collider2D> validEnemies = new List<Collider2D>();

//         foreach (SubTower sub in allSubs)
//         {
//             Collider2D[] enemiesInSubRange = Physics2D.OverlapCircleAll(sub.transform.position, sub.targetingRange, enemyMask);
//             validEnemies.AddRange(enemiesInSubRange);
//         }

//         if (validEnemies.Count > 0)
//         {
//             float closestDistance = Mathf.Infinity;
//             Transform closestEnemy = null;

//             foreach (Collider2D enemy in validEnemies)
//             {
//                 float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
//                 if (distanceToEnemy < closestDistance)
//                 {
//                     closestDistance = distanceToEnemy;
//                     closestEnemy = enemy.transform;
//                 }
//             }

//             target = closestEnemy;
//         }
//         else
//         {
//             target = null; // No target found
//         }
//     }

//     void RotateToTarget()
//     {
//         if (target == null) return;

//         Vector2 direction = (target.position - rotatingPoint.position).normalized;
//         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//         Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
//         rotatingPoint.rotation = Quaternion.RotateTowards(rotatingPoint.rotation, targetRotation, rotatingSpeed * Time.deltaTime);
//     }

//     void FireAtTarget()
//     {
//         if (target == null) return;

//         timeUntilFire -= Time.deltaTime;
//         if (timeUntilFire <= 0f)
//         {
//             Shoot();
//             timeUntilFire = 1f / aps;
//         }
//     }

//     void Shoot()
//     {
//         if (bulletPrefab != null && firingPoint != null)
//         {
//             // GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
//             // Homing bulletScript = bullet.GetComponent<Homing>();
//             // bulletScript.SetTarget(target);
//         }
//     }

//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.cyan;
//         Gizmos.DrawWireSphere(transform.position, targetingRange);
//     }
// }
