using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : Soldier
{
    public GameObject Rifle_bulletsPrefab;
    public float lockedY = 60f; // 你希望锁定的y坐标
    public string childName = "Rotation_shaft"; // 子对象的名称
    public float rotationSpeed = 960f; // 旋转速度（度/秒）


    private Transform childTransform;

    public override void Start()
    {
        unitName = "Helicopter";
        health = 150;
        attackdistance = 200;
        moveSpeed = 30.0f;
        attackInterval = 75; // 设置攻击间隔为1秒
        cost = 50;
        Vector3 position = transform.position;
        position.y = lockedY;
        transform.position = position;
        skyhit=true;
        base.Initialize();

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child.name == childName)
            {
                childTransform = child;
                break;
            }
        }
        
        if (childTransform == null)
        {
            Debug.LogError($"子对象 {childName} 没有找到！");
        }
        

    }
    public override void Update()
    {
        base.Update();
        Vector3 position = transform.position;
        position.y = lockedY;
        transform.position = position;
            if (childTransform != null)
        {
            // 沿y轴旋转
            childTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

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
