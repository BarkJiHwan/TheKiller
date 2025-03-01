using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NPCPatrolState : IState
{
    private NPCActions npc;
    private PatrolPoint[] patrolPoints;
    private int currentPointIndex = 0;
    private PatrolPoint targetPoint;
    private float patrolSpeed;
    private float rotationSpeed;

    public NPCPatrolState(NPCActions npc)
    {
        this.npc = npc;
        //patrolPoints = npc.GetPatrolPoints();
       }
    public void Enter()
    {        
        patrolPoints = npc.GetPatrolPoints();
        patrolSpeed = npc.GetPatrolSpeed();
        rotationSpeed = npc.GetComponent<NPCController>().rotationSpeed;
        npc.animator.SetFloat("Speed", 2);
        npc.animator.SetBool("Walk", true);
        targetPoint = patrolPoints[currentPointIndex];
    }
    public void Update()
    {
        if (npc.animator.GetBool("Walk"))
        {
            NPCMove();
        }
        CheckAlertState();
    }
    public void Exit()
    {
        npc.animator.SetBool("Walk", false);
    }
    private void NPCMove()
    {
        targetPoint = patrolPoints[currentPointIndex];
        
        Vector3 direction = (targetPoint.transform.position - npc.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, targetPoint.transform.position, patrolSpeed * Time.deltaTime);

        if (targetPoint.IsNPCInRange())
        {            
            MoveToNextPatrolPoint();
        }
        //else
        //{
        //    Debug.LogError("targetPoint가 null입니다.");
        //}
    }
    private void MoveToNextPatrolPoint()
    {
        int randomDirection = Random.Range(0, 2); // 0 또는 1의 값을 생성

        if (randomDirection == 0)
        {
            currentPointIndex = (currentPointIndex - 1 + patrolPoints.Length) % patrolPoints.Length;
        }
        else
        {//출발지점이 0번이라면 마지막 지점으로 이동
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
        targetPoint = patrolPoints[currentPointIndex];       
        RandomActions();
        
    }
    private void RandomActions()
    {
        int random = Random.Range(0, 5);
        Debug.Log("상태" + random);

        if (random == 0)
        {
            npc.ChangeState(NPCState.IDLE); // 상태 1
        }
        else if (random == 1)
        {
            npc.ChangeState(NPCState.ALERT); // 상태 2
        }
        else if (random == 2)
        {
            npc.ChangeState(NPCState.COVER); // 상태 3
        }
    }
    private void CheckAlertState()
    {
        Collider[] hitColliders = Physics.OverlapSphere(npc.transform.position, npc.GetAlertDistance());
        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.CompareTag("Bullet"))
            {
                npc.SetAlert(true);
                npc.ChangeState(NPCState.ALERT);
                break;
            }
        }
    }
}