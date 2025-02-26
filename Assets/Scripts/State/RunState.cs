using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private PlayerActions player;

    public RunState(PlayerActions player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetInteger("State", (int)PlayerState.RUN);
        player.animator.SetBool("Aiming", false);
        player.animator.SetFloat("Speed", 2f);
    }
    public void Exit()
    {
        player.animator.SetFloat("Speed", 0f);
    }
    public void Update()
    {
    }
}