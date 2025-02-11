using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletLifetime = 3f;
    [SerializeField] private bool isAOE = false;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemyHealth = other.gameObject.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletDamage);
        }
        if (!isAOE)
        {
            Destroy(gameObject);
        } 
    }
}