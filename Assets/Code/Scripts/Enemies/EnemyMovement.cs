using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody component for movement
    private Transform target; // Current target position
    private int pathIndex = 0; // Index for tracking path progress

    private Enemy enemy; // Reference to the Enemy script

    void Start()
    {
        enemy = GetComponent<Enemy>(); 
        if (enemy == null)
        {
            Debug.LogError("EnemyMovement: No Enemy component found on this object!");
        }

        target = LevelManager.main.path[pathIndex]; 
    }

    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;    

            if (pathIndex >= LevelManager.main.path.Length)
            {
                LevelManager.main.TakeDamage(enemy.GetPlayerDamage()); 
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        if (enemy == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * enemy.GetMoveSpeed();
    }
}
