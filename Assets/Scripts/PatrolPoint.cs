using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public float detectionRadius = 1.0f; // 감지 반경
    public LayerMask npcLayer; // NPC 레이어

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