using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    private PlayerActions action;
    private GameObject player;
    public AudioSource shoot_sound;
    private float GunCoolDown;    
    //ÅºÃ¢
    private int bulletsCount;
    public GameObject bulletPrefabs;//ÃÑ¾ËÇÁ¸®ÆÕ
    public GameObject muzzle;//¸ÓÁñ
    public GameObject target;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        action = player.GetComponent<PlayerActions>();
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
                    action.ChangeState(PlayerState.ATTACK);
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