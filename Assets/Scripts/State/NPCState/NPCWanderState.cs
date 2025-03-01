using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWanderState : IState
{
    private NPCActions npc;
    private Vector3 areaMinBounds;
    private Vector3 areaMaxBounds;

    public NPCWanderState(NPCActions npc)
    {
        this.npc = npc;
        areaMinBounds = npc.areaMinBounds;
        areaMaxBounds = npc.areaMaxBounds;
    }

    public void Enter()
    {
        npc.animator.SetBool("Wandering", true);
        MoveToRandomPositionWithinArea();
    }

    public void Update()
    {
        if (Vector3.Distance(npc.transform.position, npc.GetRandomPositionWithinArea(areaMinBounds, areaMaxBounds)) < 0.1f)
        {
            MoveToRandomPositionWithinArea();
        }
    }

    public void Exit()
    {
        npc.animator.SetBool("Wandering", false);
    }

    private void MoveToRandomPositionWithinArea()
    {
        Vector3 randomPosition = npc.GetRandomPositionWithinArea(areaMinBounds, areaMaxBounds);
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, randomPosition, npc.PatrolSpeed * Time.deltaTime);
    }
}