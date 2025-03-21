using UnityEngine;

public class PlayerCharacter : BaseTower
{

    new void Shoot()
    {
        if (target == null) return;

        // Ensure the bullet spawns at the firing point and faces the target
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target.position - firingPoint.position);
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, rotation);

        // Assign target and damage
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(target, damage);
        }
    }
}
