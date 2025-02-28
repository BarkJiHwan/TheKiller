using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAlertState : IState
{
    private NPCActions npc;
    private float alertDuration = 2f;
    private float alertTimer;
    public NPCAlertState(NPCActions npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        // 경고 애니메이션 시작
        npc.animator.SetBool("Alert", true);
        alertTimer = 0f;
        // 추가 처리 (예: 경고 사운드 재생, 주변 객체 탐색 등)
    }
    public void Update()
    {
        // 경고 상태에서의 행동 로직
        // 예: 주변을 탐색하며 플레이어를 찾는 로직

        // 예시: 경고 상태에서 순찰로 전환
        alertTimer += Time.deltaTime;
        if (alertTimer >= alertDuration)
        {
            // 일정 시간이 지나면 다시 순찰 상태로 전환
            npc.ChangeState(NPCState.PATROL);
        }
    }
    public void Exit()
    {
        // 경고 애니메이션 종료
        npc.animator.SetBool("Alert", false);
    }
}