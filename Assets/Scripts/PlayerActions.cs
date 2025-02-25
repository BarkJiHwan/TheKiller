using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public enum PlayerState
{
    None,
    IDLE,
    WALK,
    RUN,
    AIMING,    
    ATTACK,
    WARY,
    SNEAK,
    CROUCHINGRUN,
    DEATH
}
public class PlayerActions : MonoBehaviour
{
    public PlayerState playerState { get; set; }
    [SerializeField] Animator animator;

    void Start()
    {
        playerState = PlayerState.IDLE;
    }
    void Update()
    {
        UpdateAnimation();
        UpdateAction();
    }
    void UpdateAnimation()
    {
        switch (playerState)
        {
            case PlayerState.IDLE: animator.SetInteger("State", 1); break;
            case PlayerState.WALK: animator.SetInteger("State", 2); break;
            case PlayerState.RUN: animator.SetInteger("State", 3); break;
            case PlayerState.AIMING: animator.SetInteger("State", 4); break;
            case PlayerState.ATTACK: animator.SetInteger("State", 5); break;
            case PlayerState.WARY: animator.SetInteger("State", 6); break;
            case PlayerState.CROUCHINGRUN: animator.SetInteger("State", 7); break;
            case PlayerState.DEATH: animator.SetInteger("State", 8); break;
        }
    }
    void UpdateAction()
    {
        switch (playerState)
        {
            case PlayerState.IDLE: Idle(); break;
            case PlayerState.WALK: Walk(); break;
            case PlayerState.RUN: Run(); break;
            case PlayerState.AIMING: Aiming(); break;
            case PlayerState.ATTACK: Attack(); break;
            case PlayerState.WARY: Wary(); break;
            case PlayerState.CROUCHINGRUN: CrouchingRun(); break;
            case PlayerState.DEATH: Death(); break;
        }
    }
    public void Idle()
    {    }
    public void Walk()
    {
        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 0.5f);
    }
    public void Run()
    {
        animator.SetBool("Aiming", false);
        animator.SetFloat("Speed", 1f);
    }
    public void Aiming()
    {
        animator.SetBool("Squat", false);
        animator.SetFloat("Speed", 0f);
        animator.SetBool("Aiming", true);
    }
    public void Attack()
    {
        Aiming();
        animator.SetTrigger("Attack");
    }
    public void Wary() 
    {
        animator.SetBool("Squat", !animator.GetBool("Squat"));
        animator.SetBool("Aiming", false);
    }
    public void CrouchingRun() { }
    public void Death()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            animator.Play("Idle", 0);
        else
            animator.SetTrigger("Death");
    }
}
