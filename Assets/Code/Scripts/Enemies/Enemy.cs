using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] public string enemyName;
    [SerializeField] public int hitpoints = 5;
    [SerializeField] public int worth = 50;
    [SerializeField] private float originalSpeed = 2f;
    [SerializeField] public bool isConverted = false;

    public int pathIndex = 0;
    private List<Transform> path;
    public Transform target;
    
    public float moveSpeed;
    public float slowFactor = 1f;
    public bool isDestroyed = false;
    public bool isFrozen = false;
    public int originalHitpoints;

    public void Initialize(int pathID)
    {
        path = LevelManager.main.GetPath(pathID); // Assigns path based on spawn
        if (path == null || path.Count == 0)
        {
            Debug.LogError($"Enemy '{enemyName}' has no valid path!");
            Destroy(gameObject);
            return;
        }

        originalHitpoints = hitpoints;
        moveSpeed = originalSpeed;
        target = path[pathIndex]; // Start at first point
    }

    public void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex >= path.Count)
            {
                LevelManager.main.TakeDamage(hitpoints);
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }

            target = path[pathIndex];
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

    public virtual void TakeDamage(int dmg)
    {
        hitpoints -= dmg;
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
        if (pathIndex >= path.Count) return 0f;

        float distance = Vector3.Distance(transform.position, path[pathIndex].position);

        for (int i = pathIndex; i < path.Count - 1; i++)
        {
            distance += Vector3.Distance(path[i].position, path[i + 1].position);
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

    public bool CanBeConverted => !isConverted && !isDestroyed;

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
