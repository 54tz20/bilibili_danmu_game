using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : Building
{
    // Start is called before the first frame update
     public override void Start()
    {
        base.Start();
        unitName = "Vehicle";
        health = 4000;
        attackdistance = 100;
        attackInterval = 1000; // 设置攻击间隔为1秒
        cost=400;
        base.Initialize();
        
    }
    // Update is called once per frame
    public override void Update(){
        
    }
}
