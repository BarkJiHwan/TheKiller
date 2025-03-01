using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float spinSpeed = 1000f;
    public GameObject bulletImpactPrefab;      

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime);                  
        ShootRaycast();
    }

    private void ShootRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            OnBulletHit(hit);
        }
    }
    private void OnBulletHit(RaycastHit hit)
    {
        NPCController npc = hit.collider.GetComponentInParent<NPCController>();
        
        if (npc != null)
        {            
            npc.RayHit(hit.point, hit.normal, hit.collider.tag);                        
        }
        else
        {
            // NPC가 아닌 오브젝트에 충돌 시 파티클 시스템 생성
            CreateBulletImpact(hit.point, hit.normal);
        }
    }

    private void CreateBulletImpact(Vector3 position, Vector3 normal)
    {
        if(bulletImpactPrefab != null)
        {
            Quaternion rotation = Quaternion.LookRotation(normal);
            GameObject impact = Instantiate(bulletImpactPrefab, position, rotation);

            // 파티클 시스템이 완료되면 오브젝트 파괴
            StartCoroutine(DestroyImpactPrefab(impact));
        }
        else
        {
            Debug.LogWarning("bulletImpactPrefab껴야됨");
        }
    }

    IEnumerator DestroyImpactPrefab(GameObject impact)
    {
        ParticleSystem particleSystem = impact.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            while (!particleSystem.isPlaying)
            {
                yield return null;
            }
        }
        Destroy(impact);
    }
}