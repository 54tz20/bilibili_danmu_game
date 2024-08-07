
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public class Missile : Soldier
{
    public GameObject FragmentbombPrefab; // 

    public override void Start()
    {
        
        unitName = "Missile";
        health = 300;
        attackdistance = 400;
        moveSpeed = 10.0f;
        attackInterval = 500; // 设置攻击间隔为1秒
        skyhit=true;
        cost=500;
        base.Initialize();

    }
    public override void Update()
    {
         count++;
        if (transform != null)
        {
            Search();
            MoveToTarget();
            CheckDead();
            if ( target != null && health > 0)
            {
                if(count == attackInterval-200||count==attackInterval-1){
                    count++;
                    Attack();
                }
                if(count==attackInterval){
                    count = 0;
                }
            }
        }
        if (count == 10000)
        {
            count = 0;
        }

    }

    public override void Attack()
    {
        if (!MoveToTarget())
        {
            // 实际的攻击逻辑
            // 获取当前 y 值
            float Y = transform.position.y;

            // 使对象面向目标
            transform.LookAt(target);

            // 重新设置 transform.position，保持 y 值不变
            transform.position = new Vector3(transform.position.x, Y, transform.position.z);
           
            GameObject Fragmentbomb = Instantiate(FragmentbombPrefab, transform.position, Quaternion.identity);
            if (target != null && transform != null && Fragmentbomb.TryGetComponent<Fragmentbomb>(out var fragmentbomb))
            {
                fragmentbomb.SetTarget(target.position, target, transform);
            }
        }
    }
}
