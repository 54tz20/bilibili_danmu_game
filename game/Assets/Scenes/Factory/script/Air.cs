using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : Building
{
     public override void Start()
    {
        base.Start();
        unitName = "Air";
        skyhit=true;
        health = 4000;
        attackdistance = 100;
        attackInterval = 1000; // 设置攻击间隔为1秒
        cost=800;
        base.Initialize();
        
    }

    // Update is called once per frame
    public override void Update(){
        
    }
}
