using UnityEditor;
using UnityEngine;

public class Rifle_bullets : Projectile
{


    public override void Start()//射程200
    {
        speed = 200;
        lifetime = 1; // 炮弹的生命周期
        rang = 2;
        attackDamage = 2;
        base.Start();
    }
    public override void SetTarget(Vector3 position, Transform target, Transform shooter)
    {
        targetTransform = target;
        shooterTransform = shooter;
        transform.position = shooter.position; // 初始位置设为发射者的位置
        InitializeDirection(); // 子类设置目标后重新初始化方向
    }

    public override void Update()
    {
        MoveTowardsTarget();
    }
    public override void OnHitTarget()
    {
        base.OnHitTarget();
        if (targetTransform != null)
        {
            if (targetTransform.TryGetComponent<Fighter>(out var fighter))
            {
                fighter.TakeDamage(attackDamage+10);
            }
            if (targetTransform.TryGetComponent<Tank>(out var tank))
            {
                tank.TakeDamage(attackDamage+5);
            }
            if (targetTransform.TryGetComponent<Chariot>(out var chariot))
            {
                chariot.TakeDamage(attackDamage+5);
            }
             if (targetTransform.TryGetComponent<Missile>(out var missile))
            {
                missile.TakeDamage(attackDamage+5);
            }
             if (targetTransform.TryGetComponent<Building>(out var building))
            {
                building.TakeDamage(attackDamage);
            }
            if (targetTransform.TryGetComponent<AT_Infantry>(out var aT_Infantry))
            {
                aT_Infantry.TakeDamage(attackDamage+10);
            }
            if (targetTransform.TryGetComponent<Helicopter>(out var helicopter))
            {
                helicopter.TakeDamage(attackDamage);
            }
        }
    }
}