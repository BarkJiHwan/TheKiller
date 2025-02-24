using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform tr;
    public float moveSpeed = 10.0f;
    public float turnSpeed = 10.0f;
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        float pos = (v * h/2);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        tr.Rotate(0, pos, 0);
        
    }
}
