using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
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

    private PatrolPoint[] patrolPoints;
    private NPCActions npcActions;
    void Start()
    {
        npcActions = GetComponent<NPCActions>();
        InitializePatrolPoints();
        //InitializeNPCActions();
    }

    private void InitializePatrolPoints()
    {
        patrolPoints = patrolGroup.GetPatrolPoints();        
    }

    public PatrolPoint[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public void RayHit(Vector3 hitPos, Vector3 hitNormal, string hitPoint)
    {            
        if (bloodPrefab == null)
        {
            Debug.LogError("bloodPrefab이 할당되지 않았습니다.", this);
            return;
        }
        GameObject blood = Instantiate(bloodPrefab, hitPos, Quaternion.LookRotation(hitNormal));
        blood.transform.parent = transform;

        if (npcActions == null)
        {            
            Debug.LogError("npcActions가 할당되지 않았습니다.", this);
            return;
        }
        if(!npcActions.isDead)
        {
            if (hitPoint == "NPCHead")
            {
                npcActions.HeadShot();
            }
            else if (hitPoint == "NPCBody")
            {
                npcActions.BodyShot();
            }
        }
    }   
}