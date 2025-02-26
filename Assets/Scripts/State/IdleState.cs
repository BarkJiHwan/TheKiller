using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerActions player;

    public IdleState(PlayerActions player)
    {
        this.player = player;
    }
    public void Enter()
    {        
    }

    public void Exit()
    {        
    }

    public void Update()
    {
    }
}
