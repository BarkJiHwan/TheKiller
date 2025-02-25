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
    public Weapons _weapons;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (_weapons.GunTpye = null)
        {//���� ���۽� ���� �ȵ�
            SetWeapon(_weapons.name);
        }
    }

    private void Update()
    {

    }
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