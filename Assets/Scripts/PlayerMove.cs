using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float h;
    private float v;
    private float moveSpeed = 5f;
    private float jumpHeight = 5f;
    private Rigidbody rb;

    private Animator animator;    


    public Transform campos;
    public float camRotSpeed;
    Camera cam;
    float mouseX;
    float mouseY;
    
     void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void LateUpdate()
    {
        
    }
    void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");        
        transform.rotation = Quaternion.Euler(0, mouseX * camRotSpeed, 0);
        cam.transform.rotation = Quaternion.Euler(mouseY * camRotSpeed, mouseX, 0);

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 moveDiretion = new Vector3(h, 0, v).normalized;        
        float moveMegnitude = Mathf.Clamp01(Mathf.Abs(h)) + Mathf.Abs(v);        
        float moveSpeedMultiplier = (v >= 0) ? 1f : -1f;        
        rb.velocity = new Vector3(moveDiretion.x * moveSpeed, rb.velocity.y, moveDiretion.z * moveSpeed);
        
    }
}
