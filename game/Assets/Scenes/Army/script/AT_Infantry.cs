using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_Infantry : Soldier
{
     public GameObject ShellPrefab; // 
     public override void Start()
    {
        unitName = "AT_Infantry";
        health = 150;
        attackdistance = 150;
        moveSpeed = 30.0f;
        attackInterval = 1000; // 设置攻击间隔为1秒
        skyhit=true;
        cost = 50;
        skyhit=true;
        base.Initialize();
    }

     public override void Attack()
    {
        if (!MoveToTarget())
        {
           // 获取当前 y 值
            float Y = transform.position.y;

            // 使对象面向目标
            transform.LookAt(target);

            // 重新设置 transform.position，保持 y 值不变
            transform.position = new Vector3(transform.position.x, Y, transform.position.z);
         
            GameObject Shell = Instantiate(ShellPrefab, transform.position, Quaternion.identity);
            if (target != null && transform != null && Shell.TryGetComponent<Shell>(out var shell))
            {
                shell.SetTarget(target.position, target, transform);
            }
        }
    }

}
