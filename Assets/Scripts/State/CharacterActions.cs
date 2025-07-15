using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterActions : MonoBehaviour
{
    public Animator animator;
    private IState currentState;
    private Dictionary<System.Enum, IState> states;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        states = new Dictionary<System.Enum, IState>();
        InitializeStates();        
        ChangeState(NPCState.IDLE);
        
    }

    protected abstract void InitializeStates();

    private void Update()
    {
        currentState?.UpdateState();
    }

    public void ChangeState(System.Enum newState)
    {
        if (states.ContainsKey(newState))
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            currentState = states[newState];
            currentState.EnterState();
        }
        else
        {
            Debug.LogError("초기화오류: " + newState);
        }
    }

    protected void RegisterState(System.Enum stateKey, IState state)
    {
        states[stateKey] = state;
    }
}