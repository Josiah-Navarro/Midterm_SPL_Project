using UnityEngine;

public class BasicTower : BaseTower
{
    public override void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();

        if (bulletScript != null)
        {
            bulletScript.Initialize(target, damage); 
        }
    }

    protected override void FindTarget()
    {
        base.FindTarget();
    }
}
