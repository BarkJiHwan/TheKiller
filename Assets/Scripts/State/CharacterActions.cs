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
        //if (states.ContainsKey(NPCState.IDLE))
        //{
        ChangeState(NPCState.IDLE);
        //}
        //else
        //{
        //    Debug.LogError("아이들오류!");
        //}
    }

    protected abstract void InitializeStates();

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(System.Enum newState)
    {
        if (states.ContainsKey(newState))
        {
            if (currentState != null)
            {
                currentState.Exit();
            }
            currentState = states[newState];
            currentState.Enter();
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