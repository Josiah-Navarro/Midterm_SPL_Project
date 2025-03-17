using UnityEngine;

public class BufferOverflowWraith : Enemy
{
    private bool hasCloned = false; // Prevents infinite cloning
    private Color cloneColor = new Color(0.8f, 0.2f, 0.2f, 1f); // Slightly red tint for clones

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if (!hasCloned && hitpoints <= (originalHitpoints / 2) && hitpoints >=1)
        {
            hasCloned = true;
            SpawnClone();
        }
    }

    private void SpawnClone()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 spawnPosition = (Vector2)transform.position + (direction * 0.5f); 

        GameObject clone = Instantiate(gameObject, spawnPosition, Quaternion.identity);
        BufferOverflowWraith cloneScript = clone.GetComponent<BufferOverflowWraith>();

        if (cloneScript != null)
        {
            cloneScript.hitpoints = Mathf.Max(originalHitpoints / 2, 1); 
            cloneScript.moveSpeed = moveSpeed * 1.5f; 
            cloneScript.hasCloned = true;
            cloneScript.GetComponent<SpriteRenderer>().color = cloneColor; 
            Vector3 correctedPosition = clone.transform.position;
            correctedPosition.z = transform.position.z; // Match original Z
            clone.transform.position = correctedPosition;
        }
    }
}
