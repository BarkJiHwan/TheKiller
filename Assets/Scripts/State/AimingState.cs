using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingState : IState
{
    private PlayerActions player;
    public AimingState(PlayerActions player)
    {
        this.player = player;
    }
    
    public void EnterState()
    {        
        player.animator.SetFloat("Speed", 0f);
        player.animator.SetBool("Aiming", true);
    }

    public void ExitState()
    {        
        player.animator.SetBool("Aiming", false);
    }

    public void UpdateState()
    {
        player.animator.SetBool("Aiming", true);
    }
}
