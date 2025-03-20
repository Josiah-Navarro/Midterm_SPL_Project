using UnityEngine;
using System.Collections;

public class NullPointerBeetle : Enemy
{
    [Header("NullPointer Beetle Attributes")]
    [SerializeField] private float untargetableDuration = 2f;
    [SerializeField] private float untargetableCooldown = 5f; 

    private bool isUntargetable = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        StartCoroutine(UntargetableCycle());
    }

    private IEnumerator UntargetableCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(untargetableCooldown);
            isUntargetable = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white; 
            }
            yield return new WaitForSeconds(untargetableDuration);
            isUntargetable = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    public override void TakeDamage(int dmg)
    {
        if (isUntargetable) 
        {
            Debug.Log($"{enemyName} is untargetable! Damage ignored.");
            return; 
        }

        base.TakeDamage(dmg);
    }
}
