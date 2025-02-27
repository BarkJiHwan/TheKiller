using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDeathState : IState
{
    private NPCActions npc;

    public NPCDeathState(NPCActions npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        // 사망 애니메이션 시작
        npc.animator.SetTrigger("Dead");

        // NPC의 모든 행동을 멈추기
        npc.GetComponent<Rigidbody>().isKinematic = true;
        npc.GetComponent<Collider>().enabled = false;

        // 필요한 추가 처리를 수행
        // 예: 점수 증가, 사망 효과 등
    }
    public void Update()
    {
        // 사망 상태에서는 특별한 업데이트 로직이 필요 없을 수 있습니다
    }
    public void Exit()
    {
        // 사망 상태에서는 특별한 종료 로직이 필요 없을 수 있습니다
    }
}