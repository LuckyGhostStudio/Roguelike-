using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public static CameraControl instance;

    public float speed;

    public Transform target;          //目标点

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (target != null)
        {
            //相机移动
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), speed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform newTarget)  //改变目标点
    {
        target = newTarget;
    }
}
