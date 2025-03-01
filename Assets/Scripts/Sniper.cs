using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    private PlayerActions action;
    private GameObject player;
    public AudioSource shoot_sound;
    private float GunCoolDown;
    //탄창
    private int bulletsCount;
    public GameObject bulletPrefabs;//총알프리팹
    public GameObject muzzle;//머즐
    public GameObject target;
    public float bulletSpeed = 100f;
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Gizmos.DrawRay(ray.origin, ray.direction * 100);
    }
    void Shooting()
    {
        if (GunCoolDown <= 0)
        {
            if (bulletsCount > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = Camera.main.transform.position.y;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    //카메라 중앙에 최대한 가깝게 레이 발사
                    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hit;
                    Vector3 targetPosition;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //레이가 충돌한 지점
                        targetPosition = hit.point;                        
                    }
                    else
                    {
                        //레이가 충돌하지 않을 경우 적절한 거리로 설정
                        targetPosition = ray.GetPoint(1000);
                    }
                    action.ChangeState(PlayerState.ATTACK);
                    muzzle.SetActive(true);
                    var firedBullet = Instantiate(bulletPrefabs, muzzle.transform.position, muzzle.transform.rotation);
                    Vector3 direction = (targetPosition - muzzle.transform.position).normalized;

                    //총알의 진행 방향으로 회전값을 적용
                    firedBullet.transform.rotation = Quaternion.LookRotation(-direction);                    
                    firedBullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);

                    Destroy(firedBullet, 20f);

                    GunCoolDown = 1.5f;
                    bulletsCount -= 1;
                }
            }
        }
        GunCoolDown -= Time.deltaTime;
        if (GunCoolDown <= 0)
        {
            muzzle.SetActive(false);
        }
    }
}