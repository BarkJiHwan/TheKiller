using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("패트롤 설정")]
    public PatrolGroup patrolGroup;
    public float alertDistance;

    [Header("커버 설정")]
    public Transform coverPoint;

    [Header("기타 설정")]
    public GameObject bloodPrefab;
    public Vector3 areaMinBounds;
    public Vector3 areaMaxBounds;

    private PatrolPoint[] patrolPoints;
    private NPCActions npcActions;
    void Start()
    {
        InitializePatrolPoints();
        InitializeNPCActions();
    }

    private void InitializePatrolPoints()
    {
        patrolPoints = patrolGroup.GetPatrolPoints();        
    }

    private void InitializeNPCActions()
    {
        npcActions = GetComponent<NPCActions>();
        if (npcActions != null)
        {
            npcActions.Initialize(patrolPoints, coverPoint, alertDistance, areaMinBounds, areaMaxBounds);
            Debug.Log("NPC Actions initialized.");
        }
        else
        {
            Debug.LogError("NPCActions 컴포넌트가 할당되지 않았습니다.");
        }
    }

    public PatrolPoint[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public void RayHit(Vector3 hitPos, Vector3 hitNormal, string hitPoint)
    {
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        GameObject blood = Instantiate(bloodPrefab, hitPos, rot);
        blood.transform.parent = transform;
        if(hitPoint == "NPCHead")
        {
            npcActions.HeadShot();
        }
        else if(hitPoint == "NPCBody")
        {
            npcActions.BodyShot();
        }        
    }   
}