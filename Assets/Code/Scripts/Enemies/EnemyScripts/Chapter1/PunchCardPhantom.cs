using UnityEngine;

public class PunchCardPhantom : Enemy
{
    private float skipChance = 0.1f; 
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
        return pathIndex < LevelManager.main.path.Length - 2; 
    }

    private void SkipToNextPath()
    {
        target = LevelManager.main.path[pathIndex];
        transform.position = target.position; // Move enemy instantly
    }
}
