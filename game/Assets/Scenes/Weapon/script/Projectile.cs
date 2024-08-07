using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed;
    public float lifetime; // 炮弹的生命周期
    public int rang;
    public int attackDamage;
    public GameObject deathParticlesPrefab; // 死亡粒子系统预制件
    public GameObject fightParticlesPrefab; // 战斗粒子系统预制件

    public ParticleSystem fightparticle;

    public ParticleSystem deadparticle;

    public string fightparticlename;

    public string deadparticlename;

    public float fightparticlelifetime;

    public float deadparticlelifetime;

    protected Vector3 direction; // 固定方向
    public Transform targetTransform;
    public Transform shooterTransform;

    private float birthTime;

    public Transform Deadtransform;

    public Transform Fighttransform;

    public virtual void Start()
    {
        birthTime = Time.time;
        Destroy(gameObject, lifetime);
        InitializeDirection();
        InitializeParticleSystem();
    }

    // 抽象方法：设置目标
    public abstract void SetTarget(Vector3 position, Transform target, Transform shooter);

    public abstract void Update();

    // 移动到目标位置
    public virtual void MoveTowardsTarget()
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


            GameObject deadparticleObject = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Deadtransform = deadparticleObject.transform;
            Transform child = Deadtransform.Find(deadparticlename);
            Destroy(Deadtransform.gameObject, deadparticlelifetime);
            if (child!=null&&child.TryGetComponent<ParticleSystem>(out deadparticle))
            {
                deadparticle.Play();
                Destroy(Deadtransform.gameObject, deadparticlelifetime);
            }


            Destroy(gameObject); // 炮弹到达目标后销毁
        }
    }

    public virtual void MoveTrackTarget()
{
    if (targetTransform == null)
    {
        return;
    }

    // 计算每帧的移动步长
    float step = speed * Time.deltaTime;

    // 计算当前位置与目标位置之间的距离
    float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
    float dis=Vector3.Distance(shooterTransform.position, targetTransform.position);

    // 远距离移动（沿 XZ 平面移动到 Y 高度）
    if (distanceToTarget>=300 )
    {
        Vector3 targetPosition = targetTransform.position;
        UnityEngine.Debug.Log(dis);

        // 计算水平移动的目标位置
        Vector3 horizontalTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // 计算水平移动方向并更新位置
        Vector3 horizontalTargetDirection = (horizontalTargetPosition - transform.position).normalized;
        transform.position += horizontalTargetDirection * step;

        // 平滑地更新 Y 轴位置
        float targetY = Mathf.Lerp(transform.position.y, targetPosition.y+100, 0.1f);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    // 近距离移动（直接向目标位置移动）
    else
    {
        Vector3 targetDirection = (targetTransform.position - transform.position).normalized;
        direction = Vector3.Lerp(direction, targetDirection, 0.1f);
        transform.position += direction * step;
    }

    // 更新 Fighttransform 位置
    if (Fighttransform != null)
    {
        Fighttransform.position = transform.position;
    }

    // 检查是否到达目标范围并处理目标到达逻辑
    if (distanceToTarget <= rang)
    {
        OnHitTarget();
        GameObject deadparticleObject = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        Transform deadTransform = deadparticleObject.transform;
        Transform child = deadTransform.Find(deadparticlename);
        if (child != null && child.TryGetComponent<ParticleSystem>(out ParticleSystem deadparticle))
        {
            deadparticle.Play();
            Destroy(deadTransform.gameObject, deadparticlelifetime);
        }
        Destroy(gameObject);
    }
}

    // 当炮弹击中目标时调用的方法（子类可以重写）
    public virtual void OnHitTarget()
    {
        // 可在子类中实现
    }

    // 计算生成时的固定方向
    protected void InitializeDirection()
    {
        if (targetTransform != null)
        {
            direction = (targetTransform.position - transform.position).normalized;
        }
    }

    protected void InitializeParticleSystem()
    {
        if (fightParticlesPrefab != null)
        {
            GameObject fightParticleObject = Instantiate(fightParticlesPrefab, transform.position, Quaternion.identity);
            Fighttransform = fightParticleObject.transform;
            Transform child = Fighttransform.Find(fightparticlename);
            if (child!=null&&child.TryGetComponent<ParticleSystem>(out fightparticle))
            {
                fightparticle.Play();
            }
            Destroy(Fighttransform.gameObject, fightparticlelifetime);
        }
    }
}