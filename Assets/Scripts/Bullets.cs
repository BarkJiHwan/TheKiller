using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float spinSpeed = 1000f;
    private bool hasHit = false;
    private bool isHeadShot;
    private bool isBodyShot;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
        if (!hasHit)
        {            
            ShootRaycast();
        }
    }
    void ShootRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            OnBulletHit(hit);
        }
    }
    void OnBulletHit(RaycastHit hit)
    {
        NPCController npc = hit.collider.GetComponentInParent<NPCController>();
        
        if (npc != null)
        {            
            npc.RayHit(hit.point, hit.normal);

            //총알이 앞으로 나아가면서 업데이트를 돌면서 계속 호출을 하기 때문에
            //최적화면에서 bool변수에 담아서 사용
            //코드의 가독성도 챙길 수 있다.
            isHeadShot = hit.collider.CompareTag("NPCHead");
            //hit.collider.CompareTag("NPCHead") //기존코드
            isBodyShot = hit.collider.CompareTag("NPCBody") || 
                hit.collider.transform.parent.CompareTag("NPCBody");
            //hit.collider.CompareTag("NPCBody") ||
            //hit.collider.transform.parent.CompareTag("NPCBody")//기존코드

            if (isHeadShot)
            {
                npc.HeadShot();
            }
            else if (isBodyShot)
            {
                npc.BodyShot();
            }
            hasHit = true;
        }
        else
        {
            Debug.LogWarning("NPCController를 찾을 수 없습니다.");
        }
    }
}