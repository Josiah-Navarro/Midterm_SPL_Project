using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeWave : MonoBehaviour
{
    private float maxRadius;
    private float expansionTime = 1f;
    private float freezeDuration;

    private CircleCollider2D col;
    private SpriteRenderer sprite;
    private LayerMask enemyLayer; // Layer to detect enemies
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        col.isTrigger = true;
        StartCoroutine(ExpandWave());
    }

    public void SetProperties(float radius, float freezeTime, LayerMask enemyLayerMask)
    {
        maxRadius = radius;
        freezeDuration = freezeTime;
        enemyLayer = enemyLayerMask; 
    }

    IEnumerator ExpandWave()
    {
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(maxRadius, maxRadius, 1f);

        col.radius = 0f; // Start with no collider radius

        while (elapsedTime < expansionTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / expansionTime;

            transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            col.radius = Mathf.Lerp(0, maxRadius / 2f, progress); // Smooth collider expansion

            // Fade out effect
            Color currentColor = sprite.color;
            currentColor.a = Mathf.Lerp(1f, 0f, progress);
            sprite.color = currentColor;

            yield return null;
        }

        FreezeAllEnemies();

        Destroy(gameObject);
    }

    private void FreezeAllEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, maxRadius / 2f, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && !enemy.isFrozen)
            {
                enemy.Freeze(freezeDuration);
            }
        }
    }
}
