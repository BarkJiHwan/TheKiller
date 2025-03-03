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

    [Header("애니메이션 설정")]
    public RuntimeAnimatorController animatorController;    
    
    public static NPCManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {        
        int stage = GameManager.Instance.GetCurrentStage();        
        SpawnNPCsForStage(stage);
    }

    public void InitializeNPCs(PatrolGroup patrolGroup)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject npcObj = NPCPool.Instance.GetObject();
            npcObj.SetActive(false);
            NPCActions npc = npcObj.GetComponent<NPCActions>();            
            GameManager.Instance.npcs.Add(npc);
            
            Transform randomSpawnPoint = spawnPoints[GameManager.Instance.stage];
            npcObj.transform.position = randomSpawnPoint.position;
            if (patrolGroup.patrols != null && patrolGroup.patrols.Length > 0)
            {
                coverPoint = patrolGroup.patrols[0].transform;
            }
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
            npc.Initialize(patrolGroup.patrols, coverPoint, alertDistance);
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
    public void SpawnNPCsForStage(int stage)
    {
        PatrolGroup patrolGroup = GameManager.Instance.GetPatrolGroup(stage);
        if (patrolGroup != null)
        {
            InitializeNPCs(patrolGroup);
        }
    }
}
