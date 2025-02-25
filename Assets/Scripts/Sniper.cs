using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Sniper : MonoBehaviour
{    
    private PlayerWeaponSwap PWS;
    public AudioSource shoot_sound;        
    private float GunCoolDown;

    //ÅºÃ¢
    private int bulletsCount;
    public GameObject bulletPrefabs;//ÃÑ¾ËÇÁ¸®ÆÕ
    public GameObject muzzle;//¸ÓÁñ

    

    void Start()
    {
        PWS = GetComponent<PlayerWeaponSwap>();
        bulletsCount = 13;
    }

    void Update()
    {        
        Shooting();
    }

    void Shooting()
    {
        if (GunCoolDown <= 0)
        {
            if (bulletsCount > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    muzzle.SetActive(true);
                    var firedBullet = Instantiate(bulletPrefabs, muzzle.transform.position, Quaternion.identity);
                    firedBullet.GetComponent<Rigidbody>().AddForce(muzzle.transform.forward * 10, ForceMode.Impulse);
                    
                    GunCoolDown = 1.5f;
                    bulletsCount -= 1;
                }
            }
        }
        GunCoolDown -= Time.deltaTime;        
        if(GunCoolDown <= 0)
        {
            muzzle.SetActive(false);
        }
    }
}