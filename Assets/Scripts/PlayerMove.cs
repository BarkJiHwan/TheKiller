using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{    
    private Transform tr;

    public float moveSpeed = 5.0f;
    void Start()
    {
        animator = GetComponent<Animator>();
        SetWeapon(_weapons.name);        
        tr = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!Input.GetMouseButton(1))
        {
            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);            
        }        
    }
    
    [System.Serializable]
    public struct Weapons
    {
        public string name;
        public GameObject GunTpye;
        public RuntimeAnimatorController controller;
    }
    private Animator animator;
    public Transform RigPistolRight;
    public Weapons _weapons;
    
    public void SetWeapon(string name)
    {
        if (_weapons.name == name)
        {
            if (RigPistolRight.childCount > 0)
            {//�տ��ִ� �ڽĿ�����Ʈ(����) ����
                Destroy(RigPistolRight.GetChild(0).gameObject);
            }
            if (_weapons.GunTpye != null)
            {//���� �̸��� �´� �ѻ���
                GameObject newtGunTpye = (GameObject)Instantiate(_weapons.GunTpye);
                newtGunTpye.transform.parent = RigPistolRight;
                newtGunTpye.transform.localPosition = Vector3.zero;
                newtGunTpye.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            //�ѿ� �´� �ִϸ����ͷ� ���Ƴ�
            animator.runtimeAnimatorController = _weapons.controller;
        }
    }
}