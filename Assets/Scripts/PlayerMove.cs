using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    [SerializeField]public float moveSpeed = 1.0f;
    private Transform tr;
    private PlayerActions actions;
    private float defaultSpeed;
    void Start()
    {
        actions = GetComponent<PlayerActions>();
        actions.SetWeapon(actions._weapons.name);        
        tr = GetComponent<Transform>();
        defaultSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            Movement();
        }
    }
    private void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.LeftShift)&& (h !=0||v!=0))
        {
            moveSpeed = 3f;
            actions.ChangeState(PlayerState.RUN);
            Move(h, v); 
        }
        else if (h!=0 || v!=0)
        {
            moveSpeed = defaultSpeed;
            actions.ChangeState(PlayerState.WALK);
            Move(h, v);
        }
        else
        {
            actions.ChangeState(PlayerState.IDLE);
        }
    }
    private void Move(float h, float v)
    {
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
    }
}