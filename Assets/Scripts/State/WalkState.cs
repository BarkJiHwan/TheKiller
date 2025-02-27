using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
    private PlayerActions player;

    public WalkState(PlayerActions player)
    {
        this.player = player;
    }
    public void Enter()
    {        
        player.animator.SetBool("Aiming", false);
        player.animator.SetFloat("Speed", 0.5f);
    }
    public void Exit()
    { 
        player.animator.SetFloat("Speed", 0f);
    }
    public void Update()
    {
    }
}