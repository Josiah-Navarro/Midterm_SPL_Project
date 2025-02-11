using UnityEngine;

public class EchoBubble : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer bubbleSprite;
    [SerializeField] private GameObject popEffectPrefab;
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float delayBeforePop = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float moveSpeed = 5f;

    private Transform target;
    private bool attached = false;
    private float attachTime;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    void Update()
    {
        if (!attached && target != null)
        {
            MoveTowardsTarget();
        }
        else if (attached && target != null)
        {
            transform.position = target.position;
            float lifeProgress = (Time.time - attachTime) / delayBeforePop;
            UpdateColor(lifeProgress);

            if (Time.time >= attachTime + delayBeforePop)
            {
                Pop();
            }
        }
        else if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        float distance = Vector2.Distance(transform.position, target.position);
        if (distance < 0.2f) // Close enough to attach
        {
            AttachToTarget();
        }
    }

    private void AttachToTarget()
    {
        attached = true;
        attachTime = Time.time;
        rb.linearVelocity = Vector2.zero;  

        Enemy enemyScript = target.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.AddBubble(this);
        }
    }

    private void UpdateColor(float progress)
    {
        bubbleSprite.color = Color.Lerp(Color.blue, Color.red, progress);
    }

    private void Pop()
    {
        if (target != null)
        {
            Enemy enemyScript = target.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.RemoveBubble(this);
                enemyScript.TakeDamage(damage);
            }
        }

        if (popEffectPrefab != null)
        {
            Instantiate(popEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
