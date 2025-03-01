using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCoverState : IState
{
    private NPCActions npc;
    private Transform coverPoint;
    private float coverRunSpeed;
    private float closeEnoughDistance = 0.1f;
    private float coverTimer;

    public NPCCoverState(NPCActions npc)
    {
        this.npc = npc;
        coverPoint = npc.GetCoverPoint();
    }
    public void Enter()
    {
        npc.PatrolSpeed = 0;
        npc.IdleDuration = Random.Range(2f, 5f);
        npc.animator.SetBool("Cover", true);
    }
    public void Update()
    {
        coverTimer += Time.deltaTime;
        if (coverTimer >= npc.IdleDuration && npc.animator.GetBool("Patrol") == true)
        {
            coverTimer = 0;
            npc.ChangeState(NPCState.PATROL);
            return;
        }
        if (npc.animator.GetBool("Patrol") == false && Vector3.Distance(npc.transform.position, coverPoint.position) > closeEnoughDistance)
        {
            npc.animator.SetBool("Walk", false);
            npc.animator.SetBool("Run", true);
            npc.PatrolSpeed = 5;
            MoveToCoverPoint();
            return;
        }
    }
    public void Exit()
    {        
        npc.animator.SetBool("Cover", false);
    }
    private void MoveToCoverPoint()
    {
        if (coverPoint != null)
        {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, coverPoint.position, coverRunSpeed * Time.deltaTime);
        }
    }
}