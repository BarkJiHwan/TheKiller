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
    DEATH
}
public class NPCActions : CharacterActions
{
    private Transform[] patrolPoints;
    private Transform coverPoint;
    private float patrolSpeed;
    private float alertDistance;
    private bool debugLog;
    private bool isAlert;
    protected override void InitializeStates()
    {
        RegisterState(NPCState.IDLE, new NPCIdleState(this));
        RegisterState(NPCState.PATROL, new NPCPatrolState(this));
        RegisterState(NPCState.ALERT, new NPCAlertState(this));
        RegisterState(NPCState.COVER, new NPCCoverState(this));
        RegisterState(NPCState.DEATH, new NPCDeathState(this));
        
        ChangeState(NPCState.IDLE);
    }
    public void Initialize(Transform[] patrolPoints, Transform coverPoint, float patrolSpeed, float alertDistance)
    {
        this.patrolPoints = patrolPoints;
        this.coverPoint = coverPoint;
        this.patrolSpeed = patrolSpeed;
        this.alertDistance = alertDistance;        
    }

    public Transform[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public Transform GetCoverPoint()
    {
        return coverPoint;
    }

    public float GetPatrolSpeed()
    {
        return patrolSpeed;
    }

    public float GetAlertDistance()
    {
        return alertDistance;
    }

    public bool IsDebugLog()
    {
        return debugLog;
    }

    public bool IsAlert()
    {
        return isAlert;
    }

    public void SetAlert(bool alert)
    {
        isAlert = alert;
    }
}