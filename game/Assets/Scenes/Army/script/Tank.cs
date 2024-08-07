using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


public class Tank : Soldier
{
    public GameObject ShellPrefab; // 

    public override void Start()
    {
        
        unitName = "Tank";
        health = 400;
        attackdistance = 350;
        moveSpeed = 20.0f;
        attackInterval = 1000; // 设置攻击间隔为1秒
        cost=250;
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