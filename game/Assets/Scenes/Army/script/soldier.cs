using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public string unitName;
    public int health;
    public int attackdistance;
    public float moveSpeed;
    public string team;
    public float attackInterval;

    public bool skyhit;
    
    public int cost;

    public Transform target;  // 目标位置
    public GameObject deathParticlesPrefab; // 粒子系统预制件
    public ParticleSystem deadparticle;
    public Transform Deadtransform;
    public float deadparticlelifetime=2;
    public string deadparticlename;



    public int count=0;

    public virtual void Start()
    {
        Initialize();
    }

    public virtual void  Update()
    {
        count++;
        if (transform != null)
        {   Search();
            MoveToTarget();
            CheckDead();
            if (count == attackInterval && target != null && health > 0)
            {
                count = 0;
                Attack();
            }
        }
        if (count == 10000)
        {
            count = 0;
        }
    }

    public virtual void Initialize()
    {
        // 初始化目标位置为当前位置，避免空引用
        target = transform;
    }

public virtual void Search()
{
    string myTag = transform.tag;

    // 只处理有效的标签
    if (myTag == "RedTeam" || myTag == "BlueTeam")
    {
        // 目标标签取反
        string targetTag = myTag == "RedTeam" ? "BlueTeam" : "RedTeam";

        // 获取所有目标对象
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        // 筛选出根物体
        List<Transform> rootTargets = new();
        foreach (GameObject go in targets)
        {
            if (go.transform.parent == null) // 只添加没有父物体的物体
            {
                rootTargets.Add(go.transform);
            }
        }
        // 如果没有有效的目标或当前目标不在列表中，则选择最近的目标
        if (rootTargets.Count > 0)
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform potentialTarget in rootTargets)
            {
                float distance = Vector3.Distance(transform.position, potentialTarget.position);
                if(!skyhit){
                    if(potentialTarget.position.y>=40){
                        continue;
                    }
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = potentialTarget;
                }
            }
            target = closestTarget;
        }
        else
        {
            target = null;
        }
    }
}

    public virtual bool MoveToTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;

            // 将自身朝向目标位置
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget >= attackdistance)
            {
                transform.LookAt(targetPosition);
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.freezeRotation = true;
                float moveDistance = moveSpeed * Time.deltaTime;
                transform.Translate(Vector3.forward * moveDistance);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public virtual void Attack()
    {
        
    }

    public virtual void CheckDead()
    {
        if (health <= 0&&transform!=null)
        {
            GameObject deadparticleObject = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            Deadtransform = deadparticleObject.transform;
            Transform child = Deadtransform.Find(deadparticlename);
            if (child.TryGetComponent<ParticleSystem>(out deadparticle))
            {
                deadparticle.Play();
                Destroy(Deadtransform.gameObject, deadparticlelifetime);
            }
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(int attackDamage)
    {
        health -= attackDamage;
        CheckDead();
    }


}