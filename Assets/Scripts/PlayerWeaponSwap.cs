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
        {//게임 시작시 무기 안듬
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
            {//손에있는 자식오브젝트(무기) 제거
                Destroy(RigPistolRight.GetChild(0).gameObject);
            }
            if (_weapons.GunTpye != null)
            {//들어온 이름에 맞는 총생성
                GameObject newtGunTpye = (GameObject)Instantiate(_weapons.GunTpye);
                newtGunTpye.transform.parent = RigPistolRight;
                newtGunTpye.transform.localPosition = Vector3.zero;
                newtGunTpye.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            //총에 맞는 애니메이터로 갈아낌                
            animator.runtimeAnimatorController = _weapons.controller;
        }
    }
}