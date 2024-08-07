using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Soldier
{
    public Vector3 targetPosition;
    

    public override void Start()
    {
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        // 确保位置在脚本执行时是正确的
    }

    public void SetPosition(Vector3 vector3)
    {
        targetPosition = vector3;

        // 如果 Building 对象已经在场景中，更新位置
        if (gameObject.activeInHierarchy)
        {
            transform.position = targetPosition;
        }
    }
    public override bool MoveToTarget()
    {
       if (target != null)
        {
            Vector3 targetPosition = target.position;

            // 将自身朝向目标位置
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget >= attackdistance)
            {
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


    public  override void Update()
    {
        count++;
        if (transform != null)
        {   Search();
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
}