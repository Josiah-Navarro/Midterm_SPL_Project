using UnityEngine;
using System.Collections.Generic;
public class Ally : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;
    private List<Transform> path;
    private Transform target;
    private int pathIndex;

    private float moveSpeed;
    private int damage;

    public void Initialize(float speed, int allyDamage, int pathID)
    {
        moveSpeed = speed;
        damage = allyDamage;
        path = LevelManager.main.GetPath(pathID); // Get the assigned path

        if (path == null || path.Count == 0)
        {
            Debug.LogError("Ally: Assigned path is null or empty!");
            Destroy(gameObject);
            return;
        }

        pathIndex = path.Count - 1; // Start at the last point
        target = path[pathIndex];
    }

    void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex--;
            if (pathIndex < 0)
            {
                Destroy(gameObject); // Ally reaches the start and disappears
                return;
            }
            target = path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

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
