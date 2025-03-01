using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("NPC 풀 설정")]
    public int poolSize = 1;

    [Header("스폰 설정")]
    public Transform[] spawnPoints;
    public Transform coverPoint; // 커버 포인트

    [Header("NPC 행동 설정")]
    public float patrolSpeed;
    public float alertDistance;

    [Header("스테이지 설정")]
    public int stage = 1;

    [Header("영역 경계 설정")]
    public Transform panelTransform;

    [Header("애니메이션 설정")]
    public RuntimeAnimatorController animatorController;    

    private List<NPCActions> npcList = new List<NPCActions>();
    private Vector3 areaMinBounds;
    private Vector3 areaMaxBounds;

    private void Start()
    {
        SetAreaBounds();
        PatrolGroup patrolGroup = GameManager.Instance.GetPatrolGroup(stage);
        if (patrolGroup != null)
        {
            InitializeNPCs(patrolGroup);
        }
        else
        {
            Debug.LogError("Patrol group is not assigned or not found.");
        }
    }

    private void SetAreaBounds()
    {
        areaMinBounds = new Vector3(
            panelTransform.position.x - (panelTransform.localScale.x * 0.5f),
            panelTransform.position.y - (panelTransform.localScale.y * 0.5f),
            panelTransform.position.z - (panelTransform.localScale.z * 0.5f)
        );

        areaMaxBounds = new Vector3(
            panelTransform.position.x + (panelTransform.localScale.x * 0.5f),
            panelTransform.position.y + (panelTransform.localScale.y * 0.5f),
            panelTransform.position.z + (panelTransform.localScale.z * 0.5f)
        );
    }

    private void InitializeNPCs(PatrolGroup patrolGroup)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject npcObj = NPCPool.Instance.GetObject();
            npcObj.SetActive(false);
            NPCActions npc = npcObj.GetComponent<NPCActions>();
            npcList.Add(npc);

            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            npcObj.transform.position = randomSpawnPoint.position;
            
            //애니메이터 설정
            Animator animator = npcObj.GetComponent<Animator>();
            if (animator == null)
            {
                animator = npcObj.AddComponent<Animator>();
            }
            //애니메이터 컨트롤러 할당
            animator.runtimeAnimatorController = animatorController; 
            //첫번째 생성되는 앤피시 타겟 지정
            npc.isTargetNPC = (i == 0);
            //NPC 초기화
            npc.Initialize(patrolGroup.patrols, coverPoint, alertDistance, areaMinBounds, areaMaxBounds);
            //랜덤한 속도 지정
            npc.PatrolSpeed = Random.Range(2f, 5f);
            //랜덤한 총알 탐색 범위 지정
            npc.alertDistance = Random.Range(5f, 10f);
            
            NPCController npcController = npc.GetComponent<NPCController>();
            if (npcController != null)
            {//패트롤 그룹과 커버포인터 지정
                npcController.patrolGroup = patrolGroup;
                npcController.coverPoint = coverPoint;
            }
            npcObj.SetActive(true);
        }
    }    
}

//    public int poolSize = 10;
//    public Transform[] spawnPoints;
//    public Transform coverPoint; // 커버 포인트
//    public float patrolSpeed = 3.5f;
//    public float alertDistance = 5f;
//    public int stage =1;
//    public Vector3 areaMinBounds;
//    public Vector3 areaMaxBounds;

//    private List<NPCActions> npcList = new List<NPCActions>();
//    private void Start()
//    {
//        PatrolGroup patrolGroup = GameManager.Instance.GetPatrolGroup(stage); // 패트롤 포인트 그룹 가져오기
//        InitializeNPCs(patrolGroup);
//        AssignNPCBehaviors();
//    }

//    private void InitializeNPCs(PatrolGroup patrolGroup)
//    {
//        for (int i = 0; i < poolSize; i++)
//        {
//            GameObject npcObj = NPCPool.Instance.GetObject();
//            NPCActions npc = npcObj.GetComponent<NPCActions>();
//            npcList.Add(npc);

//            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
//            npcObj.transform.position = randomSpawnPoint.position;

//            // NPC 초기화
//            npc.Initialize(patrolGroup.patrols, coverPoint, patrolSpeed, alertDistance, areaMinBounds, areaMaxBounds);
//        }
//    }
//    private void AssignNPCBehaviors()
//    {
//        if (npcList.Count > 0)
//        {
//            int randomIndex = Random.Range(0, npcList.Count);
//            NPCActions targetNPC = npcList[randomIndex];
//            targetNPC.ChangeState(NPCState.PATROL); // 특정 NPC는 패트롤 상태로 전환
//        }

//        foreach (var npc in npcList)
//        {
//            if (npc.GetCurrentState() != NPCState.PATROL) // 패트롤 상태가 아닌 NPC는 랜덤하게 이동
//            {
//                npc.ChangeState(NPCState.WANDER);
//            }
//        }
//    }
//}