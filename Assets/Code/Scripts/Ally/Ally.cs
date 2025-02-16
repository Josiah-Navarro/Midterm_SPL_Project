using UnityEngine;

public class Ally : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody component for movement
    private Transform target; // Current target position
    private int pathIndex;

    private float moveSpeed;
    private int damage;

    public void Initialize(float speed, int allyDamage)
    {
        moveSpeed = speed;
        damage = allyDamage;
        pathIndex = LevelManager.main.path.Length - 1;
        target = LevelManager.main.path[pathIndex];
    }

    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex--;
            if (pathIndex < 0)
            {
                Destroy(gameObject); // Ally reaches the start and disappears
                return;
            }
            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy collidedEnemy = other.transform.GetComponent<Enemy>();
        if (collidedEnemy != null)
        {
            collidedEnemy.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
