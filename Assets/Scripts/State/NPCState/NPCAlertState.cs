using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAlertState : IState
{
    private NPCActions npc;
    private float alertTimer;
    public NPCAlertState(NPCActions npc)
    {
        this.npc = npc;
    }
    public void EnterState()
    {
        npc.PatrolSpeed = 0;
        npc.IdleDuration = Random.Range(2f, 5f);
        npc.animator.SetBool("Alert", true);
    }
    public void UpdateState()
    {
        alertTimer += Time.deltaTime;
        if (alertTimer >= npc.IdleDuration && npc.animator.GetBool("Patrol") == true)
        {
            alertTimer = 0f;
            npc.ChangeState(NPCState.PATROL);
            return;
        }
        if (alertTimer >= npc.IdleDuration && npc.animator.GetBool("Patrol") == false)
        {
            alertTimer = 0f;
            npc.ChangeState(NPCState.COVER);
            return;
        }
    }
    public void ExitState()
    {
        npc.animator.SetBool("Alert", false);
    }
}