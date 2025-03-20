using UnityEngine;
using System.Collections.Generic;
public class EnemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    private List<Transform> path;
    private Transform target;
    private int pathIndex = 0;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>(); 
        if (enemy == null)
        {
            Debug.LogError("EnemyMovement: No Enemy component found!");
            return;
        }
    }

    public void InitializePath(int pathID)
    {
        path = LevelManager.main.GetPath(pathID); // Assign path based on ID
        if (path == null || path.Count == 0)
        {
            Debug.LogError("EnemyMovement: Assigned path is null or empty!");
            return;
        }
        target = path[pathIndex];
    }

    void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;    

            if (pathIndex >= path.Count)
            {
                LevelManager.main.TakeDamage(enemy.GetPlayerDamage()); 
                EnemySpawner.onEnemyDestroy?.Invoke();
                Destroy(gameObject);
                return;
            }

            target = path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        if (enemy == null || target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * enemy.GetMoveSpeed();
    }
}
