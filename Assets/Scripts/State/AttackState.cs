using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private PlayerActions player;
    public AttackState(PlayerActions player)
    {
        this.player = player;
    }

    public void EnterState()
    {        
        player.animator.SetTrigger("Attack");
        player.animator.SetBool("Aiming", true);
    }

    public void ExitState()
    {
        player.animator.SetBool("Aiming", false);
    }

    public void UpdateState()
    {          
    }
}
