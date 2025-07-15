using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDeathState : IState
{
    private NPCActions npc;
    private bool isDead = false;

    public NPCDeathState(NPCActions npc)
    {
        this.npc = npc;
    }
    public void EnterState()
    {
        if (!isDead)
        {
            if (npc.animator.GetBool("CrawlDie") == true)
            {
                npc.animator.SetTrigger("CrawlDie");
            }
            else if (npc.animator.GetBool("CrawlDie") == false)
            {
                npc.animator.SetTrigger("Dead");
            }
            isDead = true;
        }
        //// NPC의 모든 행동을 멈추기
        //npc.GetComponent<Rigidbody>().isKinematic = true;
        //npc.GetComponent<Collider>().enabled = false;
    }
    public void UpdateState()
    {
        // 사망 상태여서 없음
    }
    public void ExitState()
    {
        // 사망 상태여서 없음
    }
}