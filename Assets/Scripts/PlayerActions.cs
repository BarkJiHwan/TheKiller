using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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
    [SerializeField] public Animator animator;
    private IState currentState;
    private Dictionary<PlayerState, IState> states;

    void Start()
    {
        animator = GetComponent<Animator>();
        states = new Dictionary<PlayerState, IState>
        {
            { PlayerState.IDLE, new IdleState(this) },
            { PlayerState.WALK, new WalkState(this) },
            { PlayerState.RUN, new RunState(this) },
            { PlayerState.AIMING, new AimingState(this) },
            { PlayerState.ATTACK, new AttackState(this) },
            { PlayerState.WARY, new WaryState(this) },
            { PlayerState.CROUCHINGRUN, new CrouchingRunState(this) },
            { PlayerState.DEATH, new DeathState(this) }
        };
        ChangeState(PlayerState.IDLE);
    }

    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = states[newState];
        currentState.Enter();
    }

    [System.Serializable]
    public struct Weapons
    {//총만들기(추가될 경우 사용)
        public string name;
        public GameObject GunTpye;
        public RuntimeAnimatorController controller;
    }
    public Transform RigPistolRight;//총이 소환될 위치(플래이어 캐릭터의 오른쪽 손)
    public Weapons _weapons;

    public void SetWeapon(string name)
    {
        if (_weapons.name == name)
        {
            if (RigPistolRight.childCount > 0)
            {//손에있는 자식오브젝트(무기) 제거
                Destroy(RigPistolRight.GetChild(0).gameObject);
            }
            if (_weapons.GunTpye != null)
            {//들어온 이름에 맞는 총생성
                GameObject newtGunTpye = (GameObject)Instantiate(_weapons.GunTpye);
                newtGunTpye.transform.parent = RigPistolRight;
                newtGunTpye.transform.localPosition = Vector3.zero;
                //생성과 동시에 X값을 90도 회전시켜 캐릭터의 손에 맞춤(캐릭터가 바뀌면 위치가 바뀔수 있음.)
                newtGunTpye.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
            //총에 맞는 애니메이터로 갈아낌
            animator.runtimeAnimatorController = _weapons.controller;
        }
    }
}
