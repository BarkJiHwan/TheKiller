using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrolState : IState
{
    private NPCActions npc;
    private Transform[] patrolPoints;
    private int currentPointIndex = 0;

    public NPCPatrolState(NPCActions npc)
    {
        this.npc = npc;
        patrolPoints = npc.GetComponent<NPCController>().GetPatrolPoints();
    }
    public void Enter()
    {
        npc.animator.SetBool("Walk", true);
        MoveToNextPatrolPoint();
    }
    public void Update()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPoint.position, npc.GetComponent<NPCController>().patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(npc.transform.position, targetPoint.position) < 0.2f)
        {
            MoveToNextPatrolPoint();
        }

    }
    public void Exit()
    {
        npc.animator.SetBool("Walk", false);
    }
    private void MoveToNextPatrolPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }
    private void CheckAlertState()
    {
        Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position, npc.GetAlertDistance());
        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.CompareTag("Player"))
            {
                npc.SetAlert(true);
                npc.ChangeState(NPCState.ALERT);
                break;
            }
        }
    }
}