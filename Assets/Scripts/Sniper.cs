using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    private MouseController mCr;
    private PlayerWeaponSwap PWS;
    public AudioSource shoot_sound;

    //에이밍 정밀도
    public float gunPrecision_notAiming = 200.0f;
    public float gunPrecision_aiming = 100.0f;
    public float gunPrecision;
    public Vector2 expandValues_crosshair;

    //총기 반동 값 변수
    public float recoilAmount_z = 0.5f;
    public float recoilAmount_x = 0.5f;
    public float recoilAmount_y = 0.5f;
    private float currentRecoilZPos;
    private float currentRecoilXPos;
    private float currentRecoilYPos;

    private float GunCoolDown;
    private float roundsPerSecond;

    //탄창
    private int bulletsCount;
    public GameObject bulletPrefabs;//총알프리팹
    public GameObject muzzel;//스나이퍼 머즐

    void Start()
    {
        PWS = GetComponent<PlayerWeaponSwap>();
        mCr = GetComponent<MouseController>();
        bulletsCount = 13;
    }

    void Update()
    {
        Shooting();
    }

    void Shooting()
    {
        //if (Input.GetMouseButtonDown(0))
        //{            
        //    ShootMethod();
        //}//탄환 쿨타임
        //    GunCoolDown -= roundsPerSecond * Time.deltaTime;
    }
    private void ShootMethod()
    {
        if (GunCoolDown <= 0)
        {
            if (bulletsCount > 0)
            {
                if (bulletPrefabs)
                {
                    muzzel.SetActive(true);
                    Instantiate(bulletPrefabs, muzzel.transform.position, muzzel.transform.rotation);
                    //shoot_sound.Play();
                }
                else
                {
                    Debug.Log("오류시 null확인.");
                    Debug.Log(muzzel + "머즐 프리팹 없음.");
                    Debug.Log(bulletPrefabs + "총알프리팹 없음.");
                    Debug.Log(shoot_sound + "총기 싸운드 없음.");
                }
                RecoilMath();
                muzzel.SetActive(false);
                GunCoolDown = 1;
                bulletsCount -= 1;
            }
        }
        else
        {
            //총알을 다 쐈는데 NPC를 다 잡지 못했다면 게임오버
            //재장전 없음
        }
    }
    public void RecoilMath()
    {
        currentRecoilZPos -= recoilAmount_z * 0.5f; // Z축 반동 감소
        currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x * 0.2f; // X축 반동 감소
        currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y * 0.2f; // Y축 반동 감소
        mCr.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision * 0.5f); // 카메라 X축 회전 감소
        mCr.wantedYRotation -= (currentRecoilXPos * gunPrecision * 0.5f); // 카메라 Y축 회전 감소

        expandValues_crosshair += Vector2.zero; //저격총은 십자선의 확장이 없음.
        GunCoolDown = 1.5f;//총 쿨타입저격 1.5~2초
        bulletsCount -= 1;
    }
}