using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private WaitForSeconds  _waitForSec;
    private IState currentState;
    private Dictionary<NPCState, IState> states;
    private PatrolPoint[] patrolPoints;
    private Transform coverPoint;

    private ScoreMgr _scoreMgr;
    private NPCManager _npcMgr;
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

    private void Awake()
    {
        if(_npcMgr == null)
        {
            _npcMgr = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        }
        if(_scoreMgr == null)
        {
            _scoreMgr = GameObject.Find("ScoreMgr").GetComponent<ScoreMgr>();
        }

        animator = GetComponent<Animator>();
        states = new Dictionary<NPCState, IState>
        {
            { NPCState.IDLE, new NPCIdleState(this)},
            { NPCState.PATROL, new NPCPatrolState(this)},
            { NPCState.ALERT, new NPCAlertState(this)},
            { NPCState.COVER, new NPCCoverState(this)},
            { NPCState.DEATH, new NPCDeathState(this)},
            { NPCState.CRAWL, new NPCCrawlState(this)},
        };
    }
    void Start()
    {
        _waitForSec = new WaitForSeconds(4f);
        originalPatrolSpeed = patrolSpeed;
        currentState = states[NPCState.IDLE];
        ChangeState(NPCState.IDLE);
    }
    void Update()
    {
        if (isDead)
        return;

        currentState.UpdateState();
    }

    public void ChangeState(NPCState newState)
    {
        if (currentState != null)
        {// 기존 상태 종료 보장받기
            currentState.ExitState();
        }
        else
        {// 예외 처리
            CheckState(newState);
        }
        if (states.ContainsKey(newState))
        {// 새로운 상태로 바꾸기
            currentState = states[newState];
            currentState.EnterState();
        }
        else
        {// 예외 처리
            CheckState(newState);
        }
    }

    private void CheckState(NPCState newState)
    {
        Debug.LogError("딕셔너리에 안담김: " + newState);
        if (states.ContainsKey(NPCState.PATROL))
        {
            currentState = states[NPCState.PATROL];
            currentState.EnterState();
        }
        else
        {
            Debug.LogError("패트롤도 없음");
            currentState = null;
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
        _scoreMgr.AddScore(200);
        Die();
    }

    public void BodyShot()
    {
        bodyHitCount++;
        if (bodyHitCount <= 1)
        {            
            ChangeState(NPCState.CRAWL);
        }
        else if (bodyHitCount >= 2)
        {
            _scoreMgr.AddScore(100);
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        StartCoroutine(DieAnimation());        
    }
    IEnumerator DieAnimation()
    {
        ChangeState(NPCState.DEATH);
        yield return _waitForSec;

        _npcMgr.RemoveNPC(this);
    }
}