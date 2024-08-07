using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot : Soldier//步兵战车
{
    public GameObject Rifle_bulletsPrefab; // 

    public override void Start()
    {

        unitName = "Chariot";
        health = 300;
        attackdistance = 170;
        moveSpeed = 35.0f;
        attackInterval = 75; // 设置攻击间隔为1秒
        cost = 300;
        base.Initialize();
    }
    public override void Attack()
    {
        if (!MoveToTarget())
        {
            // 实际的攻击逻辑
            GameObject Rifle_bullets = Instantiate(Rifle_bulletsPrefab, transform.position, Quaternion.identity);
            // 获取当前 y 值
            float Y = transform.position.y;

            // 使对象面向目标
            transform.LookAt(target);

            // 重新设置 transform.position，保持 y 值不变
            transform.position = new Vector3(transform.position.x, Y, transform.position.z);

            if (target != null && transform != null && Rifle_bullets.TryGetComponent<Rifle_bullets>(out var rifle_bullets))
            {
                rifle_bullets.SetTarget(target.position, target, transform);
            }
        }
    }
}
