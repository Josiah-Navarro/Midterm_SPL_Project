using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody component for movement

    [Header("Attribute")]
    [SerializeField] private float moveSpeed = 2f; // Speed at which the enemy moves

    private Transform target; // Current target position
    private int pathIndex = 0; // Index for tracking path progress

    void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    void Update()
    {
        // Check if the enemy has reached the target position
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++; // Move to the next path point

            // If the enemy reaches the end of the path, destroy it
            if (pathIndex >= LevelManager.main.path.Length)
            {
                // Need to Take Damage
                LevelManager.main.TakeDamage(Health.GetPlayerDamage());
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            // Update the target to the next path point
            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        // Move towards the current target position
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
}
