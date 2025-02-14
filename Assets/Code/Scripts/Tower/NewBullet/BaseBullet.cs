using UnityEngine;

public abstract class BaseBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float bulletLifetime = 3f;

    protected int damage;
    protected Transform target;

    protected virtual void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }
    protected virtual void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Initialize(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform == target)
        {
            ApplyDamage();
            Destroy(gameObject);
        }
    }

    protected virtual void ApplyDamage()
    {
        Enemy enemyHealth = target.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }
}
