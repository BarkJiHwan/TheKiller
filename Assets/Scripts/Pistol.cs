using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    private PlayerWeaponSwap PWS;
    //public MouseController mCr;
    public AudioSource shoot_sound;

    //���̹� ���е�
    public float gunPrecision_notAiming = 200.0f;
    public float gunPrecision_aiming = 100.0f;
    public float gunPrecision;
    public Vector2 expandValues_crosshair;

    //�ѱ� �ݵ� �� ����
    public float recoilAmount_z = 0.5f;
    public float recoilAmount_x = 0.5f;
    public float recoilAmount_y = 0.5f;
    private float currentRecoilZPos;
    private float currentRecoilXPos;
    private float currentRecoilYPos;

    private float GunCoolDown;
    private float roundsPerSecond;

    //źâ
    private int bulletsCount;
    public GameObject bulletPrefabs;//�Ѿ�������
    public GameObject muzzel;//�������� ����

    void Start()
    {
        PWS = GetComponent<PlayerWeaponSwap>();
        //mCr = GetComponent<MouseController>();
        bulletsCount = 25;
    }

    void Update()
    {
        Shooting();
    }

    void Shooting()
    {//�ѽ�� ��Ÿ�� �ֱ� 
     //�ѱ⿡ ���� źȯ�߻� �żҵ�        

        if (Input.GetMouseButtonDown(0))
        {
            bulletsCount = 10;
            ShootMethod();
        }//źȯ ��Ÿ��
        GunCoolDown -= roundsPerSecond * Time.deltaTime;
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
                    Debug.Log("������ nullȮ��.");
                    Debug.Log(muzzel + "���� ������ ����.");
                    Debug.Log(bulletPrefabs + "�Ѿ������� ����.");
                    Debug.Log(shoot_sound + "�ѱ� �ο�� ����.");
                }
                RecoilMath();
                muzzel.SetActive(false);
                GunCoolDown = 1;//�� ��Ÿ�� ���� 0.5�� //���� 1.5~2��
                bulletsCount -= 1;
            }
        }
        else
        {
            //�Ѿ��� �� ���µ� NPC�� �� ���� ���ߴٸ� ���ӿ���
            //������ ����
        }
    }

    //�ѱ� �ݵ� ����
    public void RecoilMath()
    {
        currentRecoilZPos -= recoilAmount_z * 1.5f; // Z�� �ݵ� ����
        currentRecoilXPos -= (Random.value - 0.5f) * recoilAmount_x * 1.2f; // X�� �ݵ� ����
        currentRecoilYPos -= (Random.value - 0.5f) * recoilAmount_y * 1.2f; // Y�� �ݵ� ����
        //mCr.wantedCameraXRotation -= Mathf.Abs(currentRecoilYPos * gunPrecision * 1.3f); // ī�޶� X�� ȸ�� ����
        //mCr.wantedYRotation -= (currentRecoilXPos * gunPrecision * 1.3f); // ī�޶� Y�� ȸ�� ����

        expandValues_crosshair += new Vector2(8, 16); // ���ڼ� Ȯ�� �� ����
        GunCoolDown = 0.5f;//�� ��Ÿ�� ���� 0.5��
        bulletsCount -= 1;
    }
}
