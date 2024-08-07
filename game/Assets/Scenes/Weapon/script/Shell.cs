

using UnityEngine;
public class Shell : Projectile
{

        public override void Start()//500
    {
        speed = 200;
        lifetime = 5; // 炮弹的生命周期
        rang = 10;
        attackDamage = 90;
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
        if(targetTransform==null){
            Destroy(gameObject);
        }
    }
    public override void OnHitTarget()
    {
        base.OnHitTarget();
        if (targetTransform != null)
        {
            if (targetTransform.TryGetComponent<Fighter>(out var fighter))
            {
                fighter.TakeDamage(attackDamage-30);
            }
            if (targetTransform.TryGetComponent<Tank>(out var tank))
            {
                tank.TakeDamage(attackDamage+20);
            }
            if (targetTransform.TryGetComponent<Chariot>(out var chariot))
            {
                chariot.TakeDamage(attackDamage);
            }
             if (targetTransform.TryGetComponent<Missile>(out var missile))
            {
                missile.TakeDamage(attackDamage);
            }
            if (targetTransform.TryGetComponent<Building>(out var building))
            {
                building.TakeDamage(500);
            }
            if (targetTransform.TryGetComponent<AT_Infantry>(out var aT_Infantry))
            {
                aT_Infantry.TakeDamage(attackDamage);
            }
            //Helicopter
            if (targetTransform.TryGetComponent<Helicopter>(out var helicopter))
            {
                helicopter.TakeDamage(attackDamage);
            }

        }
    }
}
