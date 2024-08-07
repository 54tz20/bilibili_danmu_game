using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy_industry : Building
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        unitName = "Heavy_industry";
        health = 4000;
        attackdistance = 100;
        attackInterval = 1000; // 设置攻击间隔为1秒
        cost=600;
        base.Initialize();
    }

    public override void Update(){
        
    }
}
