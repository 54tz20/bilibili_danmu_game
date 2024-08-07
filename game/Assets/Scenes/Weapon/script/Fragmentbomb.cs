

using UnityEngine;
public class Fragmentbomb : Projectile
{
    public string enemyTag; // 假设所有敌人的标签是"Enemy"
    public float damageRadius = 35f; // 伤害半径

    public override void Start()
    {
        speed = 200;
        lifetime = 4; // 炮弹的生命周期 //400
        rang = 10;
        attackDamage = 50;
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
        MoveTrackTarget();
        if (targetTransform == null)
        {
            Destroy(gameObject);
        }
    }



    public override void MoveTowardsTarget()
    {
        float step = speed * Time.deltaTime;
        transform.position += direction * step;

        if (Fighttransform != null)
        {
            Fighttransform.position = transform.position;
        }

        if (targetTransform != null && Vector3.Distance(transform.position, targetTransform.position) <= rang)
        {
            OnHitTarget();

            // 粒子效果的位置偏移
            Vector3[] offsets = new Vector3[]
            {
            new(50, 0, 0),  // 右
            new(-50, 0, 0), // 左
            new(0, 0, 50),  // 上
            new(0, 0, -50)  // 下
            };

            foreach (Vector3 offset in offsets)
            {
                // 计算偏移后的位置
                Vector3 position = transform.position + offset;

                // 在偏移位置实例化粒子效果
                GameObject deadparticleObject = Instantiate(deathParticlesPrefab, position, Quaternion.identity);
                Transform deadTransform = deadparticleObject.transform;
                Transform child = deadTransform.Find(deadparticlename);

                if (child != null && child.TryGetComponent<ParticleSystem>(out ParticleSystem deadparticle))
                {
                    deadparticle.Play();
                    Destroy(deadTransform.gameObject, deadparticlelifetime);
                }
            }

            Destroy(gameObject); // 炮弹到达目标后销毁
        }
    }

    public override void OnHitTarget()
    {
        base.OnHitTarget();
        if (targetTransform != null)
        {
            // 对直接命中的目标造成伤害
            DealDamageToTarget(targetTransform);

            // 查找附近标签相同的敌人并造成伤害
            Collider[] colliders = Physics.OverlapBox(targetTransform.position, new Vector3(damageRadius, damageRadius, damageRadius), Quaternion.identity, LayerMask.GetMask("Default")); // 假设敌人在"EnemyLayer"层上
            foreach (Collider collider in colliders)
            {
                if (collider.transform != targetTransform && collider.transform.CompareTag(enemyTag)) // 排除直接命中的目标，并确保标签匹配
                {
                    DealDamageToTarget(collider.transform);
                }
            }
        }
    }

    private void DealDamageToTarget(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            if (targetTransform.TryGetComponent<Fighter>(out var fighter))
            {
                fighter.TakeDamage(attackDamage+50);
            }
            if (targetTransform.TryGetComponent<Tank>(out var tank))
            {
                tank.TakeDamage(attackDamage-20);
            }
            if (targetTransform.TryGetComponent<Chariot>(out var chariot))
            {
                chariot.TakeDamage(attackDamage);
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
                aT_Infantry.TakeDamage(attackDamage+50);
            }
            if (targetTransform.TryGetComponent<Helicopter>(out var helicopter))
            {
                helicopter.TakeDamage(attackDamage);
            }
        }
    }
}
