using UnityEngine;

public class Homing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletLifetime = 5f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }
    void FixedUpdate()
    {
        if(!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }
    public void SetTarget(Transform _target)
    {
        if (_target == null)
        {
            Debug.LogError("HomingBullet: Target is null! Check AcePilot's Shoot() method.");
            return;
        }
        target = _target;
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        Health enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
