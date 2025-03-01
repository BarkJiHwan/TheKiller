using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.VersionControl.Asset;

public enum NPCState
{
    None,
    IDLE,
    PATROL,
    ALERT,
    COVER,
    DEATH,
    WANDER
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
    public Vector3 areaMinBounds; // 최소 경계
    public Vector3 areaMaxBounds;
    
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
            { NPCState.WANDER, new NPCWanderState(this)}
        };
   
        // 상태 초기화 확인 로그
        foreach (var state in states)
        {
            Debug.Log($"상태 추가됨: {state.Key} = {state.Value}");
        }
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

    public void Initialize(PatrolPoint[] patrolPoints, Transform coverPoint, float alertDistance, Vector3 areaMinBounds, Vector3 areaMaxBounds)
    {
        if (patrolPoints == null)
        {
            Debug.LogError("초기화 단계에서 patrolPoints 배열이 null");
        }
        this.patrolPoints = patrolPoints;
        this.coverPoint = coverPoint;
        this.alertDistance = alertDistance;
        this.areaMinBounds = areaMinBounds;
        this.areaMaxBounds = areaMaxBounds;
    }
    public PatrolPoint[] GetPatrolPoints() => patrolPoints;
    public Transform GetCoverPoint() => coverPoint;    
    public float GetAlertDistance() => alertDistance;

    public Vector3 GetRandomPositionWithinArea(Vector3 areaMinBounds, Vector3 areaMaxBounds)
    {
        float randomX = Random.Range(areaMinBounds.x, areaMaxBounds.x);
        float randomY = Random.Range(areaMinBounds.y, areaMaxBounds.y);
        float randomZ = Random.Range(areaMaxBounds.z, areaMaxBounds.z);

        return new Vector3(randomX, randomY, randomZ);
    }
    public void HeadShot()
    {
        if (isDead) return;

        Die();
    }

    public void BodyShot()
    {
        if (isDead) return;

        bodyHitCount++;
        if (bodyHitCount >= 2)
        {
            Die();
        }
        else
        {
            // 기어다니는 애니메이션 또는 특수 효과 적용
            //ChangeState(NPCState.CRAWL);
        }
    }

    private void Die()
    {
        isDead = true;
        ChangeState(NPCState.DEATH);
    }
}