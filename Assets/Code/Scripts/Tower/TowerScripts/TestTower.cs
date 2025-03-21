using UnityEngine;
using Spine;
using Spine.Unity;
using System.Collections;

public abstract class TestTower : BaseTower
{
    [Header("Spine Animation")]
    [SerializeField] protected SkeletonAnimation skeletonAnimation;

    protected override void Start()
    {
        base.Start();
        PlayAnimation("Start", false);
        skeletonAnimation.AnimationState.Event += OnAttackEvent;
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        if (target == null) return;
        PlayAnimation("Attack_Begin", false);
    }

    private void OnAttackEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "OnAttack")
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (target == null) return;
        GameObject bullet = Instantiate(towerData.bulletPrefab, firingPoint.position, Quaternion.identity);
        BaseBullet bulletScript = bullet.GetComponent<BaseBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(target, towerData.damage);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing BaseBullet component!");
        }
        PlayAnimation("Attack_End", false);
    }

    protected void PlayAnimation(string animationName, bool loop)
    {
        if (skeletonAnimation == null) return;
        skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
    }
}
