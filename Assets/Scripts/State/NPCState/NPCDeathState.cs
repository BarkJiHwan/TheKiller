using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDeathState : IState
{
    private NPCActions npc;

    public NPCDeathState(NPCActions npc)
    {
        this.npc = npc;
    }
    public void Enter()
    {
        // GameManager에서 점수 증가
        // 죽은 상황에 따라 점수 부여
        if (npc.animator.GetBool("CrawlDie") == true )
        {
            npc.animator.SetTrigger("CrawlDie");
            GameManager.Instance.AddScore(100);
        }
        if (npc.animator.GetBool("CrawlDie") == false)
        {
            npc.animator.SetTrigger("Dead");
            GameManager.Instance.AddScore(200);
        }
        // NPC의 모든 행동을 멈추기
        npc.GetComponent<Rigidbody>().isKinematic = true;
        npc.GetComponent<Collider>().enabled = false;

        
        GameObject.Destroy(npc.gameObject, 5f);
        
    }
    public void Update()
    {
        // 사망 상태여서 없음
    }
    public void Exit()
    {
        // 사망 상태여서 없음
    }
}