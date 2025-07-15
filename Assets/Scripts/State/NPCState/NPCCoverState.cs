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
        coverRunSpeed = 6f;
    }
    public void EnterState()
    {
        npc.PatrolSpeed = 0;
        coverPoint = npc.GetCoverPoint();
        npc.IdleDuration = Random.Range(2f, 5f);
        npc.animator.SetBool("Cover", true);
    }
    public void UpdateState()
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
            npc.PatrolSpeed = npc.PatrolSpeed * 0.5f;
            MoveToCoverPoint();            
            if (Vector3.Distance(npc.transform.position, coverPoint.position) <= closeEnoughDistance)
            {                
                npc.ChangeState(NPCState.PATROL);
                return;
            }
        }
    }
    public void ExitState()
    {        
        npc.animator.SetBool("Cover", false);
    }
    private void MoveToCoverPoint()
    {
        if (coverPoint != null)
        {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, coverPoint.position, coverRunSpeed * Time.deltaTime);
            npc.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.Log("커버 포인트를 못찾음");
        }
    }
}