using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : Building
{
    public GameObject ShellPrefab; 
     public override void Start()
    {
        base.Start();
        unitName = "Home";
        health = 10000;
        attackdistance = 350;
        attackInterval = 800; // 设置攻击间隔为1秒
        skyhit=true;
        cost=0;
        base.Initialize();
    }
    // Update is called once per frame
    public override void Update(){
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
    public override void Attack()
    {
        if (!MoveToTarget())
        {
            // 实际的攻击逻辑
            GameObject Shell = Instantiate(ShellPrefab, transform.position, Quaternion.identity);
            if (target != null && transform != null && Shell.TryGetComponent<Shell>(out var shell))
            {
                shell.SetTarget(target.position, target, transform);
            }
        }
    }
}
