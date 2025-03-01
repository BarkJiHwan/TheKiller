using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : IState
{
    private NPCActions npc;
    private float idleDuration;
    private float idleTimer;
    private float originalPatrolSpeed;

    public NPCIdleState(NPCActions npc)
    {
        this.npc = npc;
        originalPatrolSpeed = npc.PatrolSpeed;
    }

    public void Enter()
    {        
        idleDuration = Random.Range(2f, 5f);  // Idle 상태에서 멈춰있는 시간
        npc.PatrolSpeed = 0;
        npc.animator.SetBool("Idle", true); // Idle 애니메이션 시작
    }

    public void Update()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleDuration && !npc.animator.GetBool("InCover"))
        {
            // 일정 시간 대기 후 다음 상태로 전환
            idleTimer = 0f;
            npc.ChangeState(NPCState.PATROL);
            return;
        }
        if (idleTimer >= idleDuration && npc.animator.GetBool("Patrol"))
        {
            // 일정 시간 대기 후 다음 상태로 전환
            idleTimer = 0f;
            npc.PatrolSpeed = originalPatrolSpeed;
            npc.ChangeState(NPCState.PATROL);
            return;
        }
    }

    public void Exit()
    {
        npc.animator.SetBool("Idle", false); // Idle 애니메이션 종료
    }
}