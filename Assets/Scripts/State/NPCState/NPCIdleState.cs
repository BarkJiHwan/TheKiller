using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIdleState : IState
{
    private NPCActions npc;
    private float idleDuration;
    private float idleTimer;

    public NPCIdleState(NPCActions npc)
    {
        this.npc = npc;
        idleDuration = Random.Range(2f, 5f);  // Idle 상태에서 멈춰있는 시간
    }

    public void Enter()
    {
        npc.animator.SetBool("Walk", false);
        npc.animator.SetBool("Idle", true); // Idle 애니메이션 시작
        idleTimer = 0f;
    }

    public void Update()
    {
        idleTimer += Time.deltaTime;
        Debug.Log(idleTimer);
        Debug.Log(idleDuration);
        if (idleTimer >= idleDuration)
        {            
            // 일정 시간 대기 후 다음 상태로 전환
            npc.ChangeState(NPCState.PATROL);
        }
    }

    public void Exit()
    {
        npc.animator.SetBool("Idle", false); // Idle 애니메이션 종료
    }
}