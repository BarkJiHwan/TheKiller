using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCoverState : IState
{
    private NPCActions npc;
    private Transform coverPoint;

    public NPCCoverState(NPCActions npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        // 커버 애니메이션 시작
        npc.animator.SetBool("isInCover", true);

        // 커버로 이동하는 로직
        MoveToCoverPoint();
    }
    public void Update()
    {
        if (npc.transform.position != coverPoint.position)
        {
            MoveToCoverPoint();
        }
    }
    public void Exit()
    {
        // 커버 애니메이션 종료
        npc.animator.SetBool("InCover", false);
    }
    private void MoveToCoverPoint()
    {
        if (coverPoint != null)
        {
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, coverPoint.position, npc.GetComponent<NPCController>().patrolSpeed * Time.deltaTime);
        }

    }
}
