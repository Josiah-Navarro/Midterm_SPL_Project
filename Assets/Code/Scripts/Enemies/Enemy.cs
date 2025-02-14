using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] public string enemyName;
    [SerializeField] private int hitpoints = 5;
    [SerializeField] private int worth = 50;
    [SerializeField] private float originalSpeed = 2f;

    private float distanceToEnd;
    private float moveSpeed;
    private float slowFactor = 1f;
    public bool isDestroyed = false;
    public bool isFrozen = false;

    private Transform target;
    private int pathIndex = 0;

    private List<EchoBubble> attachedBubbles = new List<EchoBubble>();

    void Start()
    {
        moveSpeed = originalSpeed;
        target = LevelManager.main.path[pathIndex];
    }

    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= LevelManager.main.path.Length)
            {
                LevelManager.main.TakeDamage(hitpoints);
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            target = LevelManager.main.path[pathIndex];
        }
    }

    private void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * (moveSpeed * slowFactor);
        }
    }

    public void TakeDamage(int dmg)
    {
        hitpoints -= dmg;
        Debug.Log("Enemy Took "+ dmg);
        if (hitpoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(worth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void Freeze(float duration)
    {
        if (isFrozen) return;

        isFrozen = true;
        moveSpeed = 0;
        StartCoroutine(UnfreezeAfter(duration));
    }
    
    public float GetDistanceToEnd()
    {
        if (pathIndex >= LevelManager.main.path.Length) return 0f; // Already at the end

        float distance = Vector3.Distance(transform.position, LevelManager.main.path[pathIndex].position);

        for (int i = pathIndex; i < LevelManager.main.path.Length - 1; i++)
        {
            distance += Vector3.Distance(LevelManager.main.path[i].position, LevelManager.main.path[i + 1].position);
        }

        return distance;
    }


    private IEnumerator UnfreezeAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        moveSpeed = originalSpeed;
    }

    public void ApplySlow(float factor, float duration)
    {
        if (isFrozen) return;

        if (factor < slowFactor)
        {
            slowFactor = factor;
            StartCoroutine(RemoveSlowAfter(duration));
        }
    }

    private IEnumerator RemoveSlowAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        slowFactor = 1f;
    }

    public void AddBubble(EchoBubble bubble)
    {
        attachedBubbles.Add(bubble);
    }

    public void RemoveBubble(EchoBubble bubble)
    {
        attachedBubbles.Remove(bubble);
    }

    private void OnDestroy()
    {
        foreach (var bubble in attachedBubbles)
        {
            Destroy(bubble.gameObject);
        }
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public int GetPlayerDamage()
    {
        return hitpoints;
    }

    public void SetMoveSpeed(float _movespeed)
    {
        moveSpeed = _movespeed;
    }

    public void RestoreSpeed()
    {
        moveSpeed = originalSpeed;
    }
}
