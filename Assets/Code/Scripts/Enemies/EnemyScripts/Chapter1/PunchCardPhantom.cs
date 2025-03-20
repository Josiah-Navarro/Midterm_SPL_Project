using UnityEngine;

public class PunchCardPhantom : Enemy
{
    private float skipChance = 0.1f; // 10% chance to skip
    private float skipCooldown = 5f;
    private float nextSkipTime = 0f;

    new void Update()
    {
        base.Update();

        if (Time.time >= nextSkipTime && Random.value < skipChance && CanSkipPath())
        {
            SkipToNextPath();
            nextSkipTime = Time.time + skipCooldown;
        }
    }

    private bool CanSkipPath()
    {
        return pathIndex < LevelManager.main.GetPath(pathIndex).Count - 2; 
    }

    private void SkipToNextPath()
    {
        pathIndex += 2; // Skip one waypoint
        if (pathIndex >= LevelManager.main.GetPath(pathIndex).Count)
        {
            pathIndex = LevelManager.main.GetPath(pathIndex).Count - 1; // Stay at last waypoint
        }
        target = LevelManager.main.GetPath(pathIndex)[pathIndex];
        transform.position = target.position; // Move enemy instantly
    }
}
