using UnityEngine;
using System.Collections;

public abstract class BaseWave : MonoBehaviour
{
    public float maxRadius;
    private float expansionTime = 1f;
    private CircleCollider2D col;
    private SpriteRenderer sprite;
    public LayerMask enemyLayer;
    protected virtual void Start()
    {
        col = GetComponent<CircleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        col.isTrigger = true;
        StartCoroutine(ExpandWave());
    }
    public virtual void SetProperties(float radius, LayerMask enemyMask)
    {
        maxRadius = radius;
        enemyLayer = enemyMask;
    }
    IEnumerator ExpandWave()
    {
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = new Vector3(maxRadius, maxRadius, 1f);

        col.radius = 0f;

        while(elapsedTime < expansionTime)
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
        TriggerEffect();
        Destroy(gameObject);
    }
    public virtual void TriggerEffect()
    {
        Debug.Log("Trigger Effect");
    }
}