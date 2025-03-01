using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCoverState : IState
{
    private NPCActions npc;
    private Transform coverPoint;
    private float coverRunSpeed;
    private float closeEnoughDistance = 0.1f;

    public NPCCoverState(NPCActions npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        coverPoint = npc.GetCoverPoint();
        coverRunSpeed = npc.GetComponent<NPCController>().runSpeed;
        // 커버 애니메이션 시작
        npc.animator.SetBool("InCover", true);
        npc.animator.SetFloat("Speed", coverRunSpeed);
        // 커버로 이동하는 로직
        MoveToCoverPoint();
    }
    public void Update()
    {
        if (Vector3.Distance(npc.transform.position, coverPoint.position) > closeEnoughDistance)
        {
            MoveToCoverPoint();
        }
    }
    public void Exit()
    {
        // 커버 애니메이션 종료
        npc.animator.SetBool("InCover", false);
        npc.animator.SetFloat("Speed", 0f);
        npc.animator.SetTrigger("Exit");
    }
    private void MoveToCoverPoint()
    {
        if (coverPoint != null)
        {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, coverPoint.position, coverRunSpeed * Time.deltaTime);
        }
    }
}