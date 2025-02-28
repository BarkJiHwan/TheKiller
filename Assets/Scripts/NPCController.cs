using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("패트롤 설정")]
    public Transform patrolGroup;
    public float patrolSpeed = 3;
    public float rotationSpeed = 3;
    public float runSpeed = 5f;
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
        if (patrolGroup != null)
        {
            int childCount = patrolGroup.childCount;
            patrolPoints = new PatrolPoint[childCount];
            for (int i = 0; i < childCount; i++)
            {
                PatrolPoint patrolPoint = patrolGroup.GetChild(i).GetComponent<PatrolPoint>();
                if (patrolPoint != null)
                {
                    patrolPoints[i] = patrolPoint;            
                }
                else
                {
                    Debug.LogWarning("PatrolPoint component not found on child object " + i);
                }
            }
        }
        else
        {
            Debug.LogWarning("Patrol group is not assigned.");
            patrolPoints = new PatrolPoint[0]; // 빈 배열로 초기화
        }
        if (patrolGroup != null)
        {
            int childCount = patrolGroup.childCount;
            patrolPoints = new PatrolPoint[childCount];
            for (int i = 0; i < childCount; i++)
            {
                patrolPoints[i] = patrolGroup.GetChild(i).GetComponent<PatrolPoint>();
            }
        }
        else
        {
            Debug.LogWarning("Patrol group is not assigned.");
            patrolPoints = new PatrolPoint[0]; // 빈 배열로 초기화
        }
    }

    private void InitializeNPCActions()
    {
        npcActions = GetComponent<NPCActions>();
        if (npcActions != null)
        {
            npcActions.Initialize(patrolPoints, coverPoint, patrolSpeed, alertDistance, areaMinBounds, areaMaxBounds);
        }
        else
        {
            Debug.LogError("NPCActions component is not assigned.");
        }
    }

    public PatrolPoint[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public void RayHit(Vector3 hitPos, Vector3 hitNormal)
    {
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        GameObject blood = Instantiate(bloodPrefab, hitPos, rot);
        blood.transform.parent = transform;
    }

    public void HeadShot()
    {
        // 데스 애니메이션
        Debug.Log("Head shot!");
    }

    public void BodyShot()
    {
        // 기어다니는 애니메이션
        Debug.Log("Body shot!");
    }
}