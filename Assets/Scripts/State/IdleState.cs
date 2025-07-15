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
    public void EnterState()
    {        
    }

    public void ExitState()
    {        
    }

    public void UpdateState()
    {
    }
}
