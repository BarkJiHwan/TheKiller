using System.Collections;
using System.Collections.Generic;
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
    public Animator animator;
    private IState currentState;
    private Dictionary<NPCState, IState> states;
    private PatrolPoint[] patrolPoints;
    private Transform coverPoint;
    private float patrolSpeed;
    private float alertDistance;
    private bool isAlert;

    public Vector3 areaMinBounds;
    public Vector3 areaMaxBounds;

    void Start()
    {
        animator = GetComponent<Animator>();
        states = new Dictionary<NPCState, IState>
        {
            { NPCState.IDLE, new NPCIdleState(this)},
            { NPCState.PATROL, new NPCPatrolState(this)},
            { NPCState.ALERT, new NPCAlertState(this)},
            { NPCState.COVER, new NPCCoverState(this)},
            { NPCState.DEATH, new NPCDeathState(this)},
            { NPCState.WANDER, new NPCWanderState(this)}
        };
        // 초기 상태 설정
        ChangeState(NPCState.IDLE);
        if (currentState == null)
        {
            Debug.LogError("초기화 안됨");
        }
    }
        void Update()
    {
        currentState.Update();
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

    public void Initialize(PatrolPoint[] patrolPoints, Transform coverPoint, float patrolSpeed, float alertDistance, Vector3 areaMinBounds, Vector3 areaMaxBounds)
    {        
        this.patrolPoints = patrolPoints;
        this.coverPoint = coverPoint;
        this.patrolSpeed = patrolSpeed;
        this.alertDistance = alertDistance;
        this.areaMinBounds = areaMinBounds;
        this.areaMaxBounds = areaMaxBounds;
    }

    public PatrolPoint[] GetPatrolPoints() => patrolPoints;
    public Transform GetCoverPoint() => coverPoint;
    public float GetPatrolSpeed() => patrolSpeed;
    public float GetAlertDistance() => alertDistance;
    public bool IsAlert() => isAlert;
    public void SetAlert(bool alert) => isAlert = alert;

    public Vector3 GetRandomPositionWithinArea(Vector3 areaMinBounds, Vector3 areaMaxBounds)
    {
        float randomX = Random.Range(areaMinBounds.x, areaMaxBounds.x);
        float randomY = Random.Range(areaMinBounds.y, areaMaxBounds.y);
        float randomZ = Random.Range(areaMaxBounds.z, areaMaxBounds.z);

        return new Vector3(randomX, randomY, randomZ);
    }
    //private PatrolPoint[] patrolPoints;
    //private Transform coverPoint;
    //private float patrolSpeed;
    //private float alertDistance;
    //private bool isAlert;
    //private NPCState currentState;
    //private Dictionary<NPCState, IState> stateDictionary;

    //public Vector3 areaMinBounds;
    //public Vector3 areaMaxBounds;

    //protected override void InitializeStates()
    //{
    //    stateDictionary = new Dictionary<NPCState, IState>();
    //    RegisterState(NPCState.IDLE, new NPCIdleState(this));
    //    RegisterState(NPCState.PATROL, new NPCPatrolState(this));
    //    RegisterState(NPCState.ALERT, new NPCAlertState(this));
    //    RegisterState(NPCState.COVER, new NPCCoverState(this));
    //    RegisterState(NPCState.DEATH, new NPCDeathState(this));
    //    RegisterState(NPCState.WANDER, new NPCWanderState(this));

    //    currentState = NPCState.IDLE;
    //}

    //public void Initialize(PatrolPoint[] patrolPoints, Transform coverPoint, float patrolSpeed, float alertDistance, Vector3 areaMinBounds, Vector3 areaMaxBounds)
    //{
    //    this.patrolPoints = patrolPoints;
    //    this.coverPoint = coverPoint;
    //    this.patrolSpeed = patrolSpeed;
    //    this.alertDistance = alertDistance;
    //    this.areaMinBounds = areaMinBounds;
    //    this.areaMaxBounds = areaMaxBounds;
    //    ChangeState(NPCState.IDLE);
    //}

    //public void ChangeState(NPCState newState)
    //{
    //    if (stateDictionary.ContainsKey(currentState))
    //    {
    //        stateDictionary[currentState].Exit();
    //    }

    //    currentState = newState;

    //    if (stateDictionary.ContainsKey(currentState))
    //    {
    //        stateDictionary[currentState].Enter();
    //    }
    //    else
    //    {
    //        Debug.LogError("State not found: " + currentState);
    //    }
    //}

    //public PatrolPoint[] GetPatrolPoints() => patrolPoints;
    //public Transform GetCoverPoint() => coverPoint;
    //public float GetPatrolSpeed() => patrolSpeed;
    //public float GetAlertDistance() => alertDistance;
    //public bool IsAlert() => isAlert;
    //public void SetAlert(bool alert) => isAlert = alert;
    //public NPCState GetCurrentState() => currentState;
    //public void SetCurrentState(NPCState state) => currentState = state;

    //public Vector3 GetRandomPositionWithinArea(Vector3 areaMinBounds, Vector3 areaMaxBounds)
    //{
    //    float randomX = Random.Range(areaMinBounds.x, areaMaxBounds.x);
    //    float randomY = Random.Range(areaMinBounds.y, areaMaxBounds.y);
    //    float randomZ = Random.Range(areaMinBounds.z, areaMaxBounds.z);

    //    return new Vector3(randomX, randomY, randomZ);
    //}
    //private void RegisterState(NPCState stateKey, IState state)
    //{
    //    stateDictionary[stateKey] = state;
    //    Debug.Log("State registered: " + stateKey);

    //}
}