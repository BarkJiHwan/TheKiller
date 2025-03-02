using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCrawlState : IState
{
    private NPCActions npc;
    private Transform coverPoint;    
    private float closeEnoughDistance = 0.1f;

    public NPCCrawlState(NPCActions npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {        
        coverPoint = npc.GetCoverPoint();
        npc.PatrolSpeed = 1f;
        npc.animator.SetBool("Patrol",false);
        npc.animator.SetBool("Walk", false);
        npc.animator.SetBool("Crawl", true);
    }
    public void Update()
    {
        if (!npc.isDead && npc.animator.GetBool("Crawl") == true && Vector3.Distance(npc.transform.position, coverPoint.position) > closeEnoughDistance)
        {
            Vector3 direction = (coverPoint.transform.position - npc.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, lookRotation, Time.deltaTime * npc.rotationSpeed);
            npc.transform.position = Vector3.MoveTowards(npc.transform.position, coverPoint.position, npc.PatrolSpeed * Time.deltaTime);
            if (Vector3.Distance(npc.transform.position, coverPoint.position) <= closeEnoughDistance)
            {                
                npc.animator.SetTrigger("Exit");
                return;
            }
        }
        else if (npc.isDead)
        {
            npc.animator.SetBool("CrawlDie", true);
            npc.ChangeState(NPCState.DEATH);
            return;
        }
    }

    public void Exit()
    {
        
    }
}
