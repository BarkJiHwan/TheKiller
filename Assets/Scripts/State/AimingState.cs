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
    
    public void Enter()
    {
        player.animator.SetInteger("State", (int)PlayerState.AIMING);
        player.animator.SetFloat("Speed", 0f);
        player.animator.SetBool("Aiming", true);
    }

    public void Exit()
    {        
        player.animator.SetBool("Aiming", false);
    }

    public void Update()
    {
        player.animator.SetBool("Aiming", true);
    }
}
