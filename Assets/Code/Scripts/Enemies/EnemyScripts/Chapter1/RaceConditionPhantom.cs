using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceConditionRevenant : Enemy
{
    [Header("Race Condition Behavior")]
    public float minSpeedMultiplier = 0.5f;
    public float maxSpeedMultiplier = 2.0f;
    public float speedChangeInterval = 2.0f;
    public float disruptionRadius = 3.0f;
    public float disruptionInterval = 1.0f;

    private float baseSpeed;
    private Coroutine disruptionCoroutine;
    private HashSet<BaseTower> affectedTowers = new HashSet<BaseTower>();

    void Start()
    {
        baseSpeed = moveSpeed;

        // Start modifying speed and disrupting tower targeting
        StartCoroutine(ChangeSpeedRoutine());
        disruptionCoroutine = StartCoroutine(DisruptTowersRoutine());
    }

    private IEnumerator ChangeSpeedRoutine()
    {
        while (true)
        {
            float randomMultiplier = Random.Range(minSpeedMultiplier, maxSpeedMultiplier);
            moveSpeed = baseSpeed * randomMultiplier;
            yield return new WaitForSeconds(speedChangeInterval);
        }
    }

    private IEnumerator DisruptTowersRoutine()
    {
        while (true)
        {
            Collider2D[] towersInRange = Physics2D.OverlapCircleAll(transform.position, disruptionRadius);
            HashSet<BaseTower> currentTowers = new HashSet<BaseTower>();

            foreach (Collider2D col in towersInRange)
            {
                BaseTower tower = col.GetComponent<BaseTower>();
                if (tower != null)
                {
                    currentTowers.Add(tower);
                    tower.RandomizeTargetingMode();
                }
            }

            // Reset targeting mode for towers that are no longer in range
            foreach (BaseTower tower in affectedTowers)
            {
                if (!currentTowers.Contains(tower))
                {
                    tower.ResetTargetingMode();
                }
            }

            affectedTowers = currentTowers;
            yield return new WaitForSeconds(disruptionInterval);
        }
    }

    protected void OnDestroy()
    {
        if (disruptionCoroutine != null)
        {
            StopCoroutine(disruptionCoroutine);
        }

        // Ensure all affected towers reset their targeting mode when this enemy dies
        foreach (BaseTower tower in affectedTowers)
        {
            tower.ResetTargetingMode();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, disruptionRadius);
    }
#endif
}
