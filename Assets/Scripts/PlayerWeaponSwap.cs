using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static PlayerController;

public class PlayerWeaponSwap : MonoBehaviour
{
    [System.Serializable]
    public struct Weapons
    {
        public string name;
        public GameObject GunTpye;
        public RuntimeAnimatorController controller;
    }
    private Animator animator;    
    public Transform RigPistolRight;
    public Weapons[] _weapons;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (_weapons.Length > 0)
        {//���� ���۽� ���� �ȵ�
            SetWeapon(_weapons[0].name);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(_weapons[0].name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(_weapons[1].name);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(_weapons[2].name);
        }
    }
    public void SetWeapon(string name)
    {
        foreach (Weapons weapon in _weapons)
        {
            if (weapon.name == name)
            {
                if (RigPistolRight.childCount > 0)
                {//�տ��ִ� �ڽĿ�����Ʈ(����) ����
                    Destroy(RigPistolRight.GetChild(0).gameObject);
                }
                if (weapon.GunTpye != null)
                {//���� �̸��� �´� �ѻ���
                    GameObject newtGunTpye = (GameObject)Instantiate(weapon.GunTpye);
                    newtGunTpye.transform.parent = RigPistolRight;
                    newtGunTpye.transform.localPosition = Vector3.zero;
                    newtGunTpye.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                //�ѿ� �´� �ִϸ����ͷ� ���Ƴ�                
                animator.runtimeAnimatorController = weapon.controller;
            }
        }
    }
}
