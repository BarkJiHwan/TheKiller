using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Controls;
using static UnityEditor.VersionControl.Asset;

public enum NPCState
{
    None,
    IDLE,
    PATROL,
    ALERT,
    COVER,
    DEATH,
    CRAWL
}
public class NPCActions : MonoBehaviour
{
    [Header("NPC 설정")]
    [SerializeField] private float patrolSpeed = 3f; // 기본 패트롤 속도
    public float originalPatrolSpeed;
    public float rotationSpeed = 3f; // 회전 속도
    public float runSpeed = 5f; // 달리기 속도
    public float alertDistance; // 경고 거리
    private float idleDuration;
    
    [Header("타겟 설정")]
    public bool isTargetNPC;
    [Header("엔피시 생존 여부 시작은 false")]
    public bool isDead = false;
    [Header("바디 2방 헤드 1방")]
    public int bodyHitCount = 0;

    public Animator animator;

    private IState currentState;
    private Dictionary<NPCState, IState> states;
    private PatrolPoint[] patrolPoints;
    private Transform coverPoint;

    public float PatrolSpeed
    {
        get => patrolSpeed;
        set => patrolSpeed = value;
    }
    public float IdleDuration
    {
        get => idleDuration;
        set => idleDuration = value;
    }

    void Start()
    {        
        animator = GetComponent<Animator>();
        originalPatrolSpeed = patrolSpeed;
        states = new Dictionary<NPCState, IState>
        {
            { NPCState.IDLE, new NPCIdleState(this)},
            { NPCState.PATROL, new NPCPatrolState(this)},
            { NPCState.ALERT, new NPCAlertState(this)},
            { NPCState.COVER, new NPCCoverState(this)},
            { NPCState.DEATH, new NPCDeathState(this)},
            { NPCState.CRAWL, new NPCCrawlState(this)},
        };
        ChangeState(NPCState.IDLE);
    }
    void Update()
    {
        if(isTargetNPC)
        {
            ApplySpecialEffect();
        }
        currentState.Update();
    }

    void ApplySpecialEffect()
    {

    }

    public void ChangeState(NPCState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        if (states.ContainsKey(newState)) 
        {
            currentState = states[newState];
            currentState.Enter();
        }
        else
        {
            Debug.LogError("딕셔너리에 안담김: " + newState);
        }
        if (currentState == null)
        {
            Debug.LogError("커런트 오류 " + newState);

        }
    }

    public void Initialize(PatrolPoint[] patrolPoints, Transform coverPoint, float alertDistance)
    {
        if (patrolPoints == null)
        {
            Debug.LogError("초기화 단계에서 patrolPoints 배열이 null");
        }
        this.patrolPoints = patrolPoints;
        this.coverPoint = coverPoint;
        this.alertDistance = alertDistance;
    }
    public PatrolPoint[] GetPatrolPoints() => patrolPoints;
    public Transform GetCoverPoint() => coverPoint;    
    public float GetAlertDistance() => alertDistance;

    public void HeadShot()
    {
        Die();
    }

    public void BodyShot()
    {   
        bodyHitCount++;
        if (bodyHitCount <= 1)
        {
            ChangeState(NPCState.CRAWL);
        }
        else if(bodyHitCount >= 2)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        GameManager.Instance.RemoveNPC(this);
        ChangeState(NPCState.DEATH);
    }
}