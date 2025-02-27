using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class CharacterActions : MonoBehaviour
{
    public Animator animator;
    private IState currentState;
    private Dictionary<System.Enum, IState> states;
        
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        states = new Dictionary<System.Enum, IState>();
        //InitializeStates();
        if(states.ContainsKey(NPCState.IDLE))
        {
            ChangeState(NPCState.IDLE);
        }
        else
        {
            Debug.Log("아이들오류!");
        }
    }

    protected abstract void InitializeStates();

    void Update()
    {
        currentState?.Update();
    }
    public void ChangeState(System.Enum newstate)
    {
        if (states.ContainsKey(newstate))
        {
            currentState.Exit();
            currentState = states[newstate];
            currentState.Enter();
        }
        else
        {
            Debug.Log("초기화오류" + newstate);
        }
    }
    protected void RegisterState(System.Enum stateKey, IState state)
    {
        states[stateKey] = state;
    }
}
