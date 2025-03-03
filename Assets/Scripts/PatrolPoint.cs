using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public float detectionRadius = 1.0f; // 감지 반경
    public LayerMask npcLayer; // NPC 레이어

    public GameObject particlePrefab; // 파티클 프리팹

    private GameObject particleInstance;
    private void Start()
    {
        if (particlePrefab != null)
        {
            // 파티클 프리팹 인스턴시에이트
            particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    public bool IsNPCInRange(NPCActions npc)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, npcLayer);        
        foreach (Collider hitCol in hitColliders)
        {//닿은 npc에게만 트루를 적용
            if (hitCol.CompareTag("NPCBody")&& hitCol.gameObject == npc.gameObject)
            {
                return true;
            }
        }
        return false;
    }
}