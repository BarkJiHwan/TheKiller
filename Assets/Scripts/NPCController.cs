using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform patrolGroup;
    public float patrolSpeed = 3;    
    public float alertDistance;
    public Transform coverPoint;
    public GameObject bloodPrefab;
    private Transform[] patrolPoints;
    private NPCActions npcActions;
    void Start()
    {
        patrolPointsArray();
        npcActions = GetComponent<NPCActions>();
        npcActions.Initialize(patrolPoints, coverPoint, patrolSpeed, alertDistance);
    }

    void Update()
    {        
    }
    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }
    void patrolPointsArray()
    {
        patrolPoints = new Transform[patrolGroup.childCount];
        for (int i = 0; i < patrolGroup.childCount; i++)
        {
            patrolPoints[i] = patrolGroup.GetChild(i);
            Debug.Log(i + "개담겼습니다.");
        }
    }        

    public void RayHit(Vector3 hitPos, Vector3 hitNormal)
    {
        Quaternion rot = Quaternion.LookRotation(hitNormal);
        var Blood = Instantiate(bloodPrefab, hitPos, rot);
        Blood.transform.parent = transform;
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
