using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public partial class PlayerController : ObjectScript
{
    private PlayerCamera playerCam;     // 플레이어 카메라
    private Rigidbody playerRB;         // 플레이어 리지드바디      

    public static event Action OnPlayerDeath; // 플레이어의 죽음을 알리는 이벤트

    private bool isDead;

    // 입력 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }  // Input System Player 입력
    public Vector3 MoveInput { get { return PlayerInput.Movement.ReadValue<Vector2>(); } }      // wasd 입력
    public Vector2 MouseDelta { get { return PlayerInput.Look.ReadValue<Vector2>(); } }         // 마우스 좌표
    public bool AttackTrigger { get { return PlayerInput.Attack.triggered; } }                  // 공격
    public bool JumpTrigger { get { return PlayerInput.Jump.triggered; } }                      // 점프                                                                                
    public bool InteractTrigger { get { return PlayerInput.Interact.triggered; } }              // 상호작용
    public bool SupportUITrigger { get { return PlayerInput.SupportUI.triggered; } }            // AI 대화창


    public void PlayerDie()
    {
        if (isDead)
            return;
        isDead = true;
        
        OnPlayerDeath?.Invoke();
        // 사망 애니메이션
        // 기타 사망 로직

        PlayerInput.Disable();
        this.enabled = false;
    }

    private void UpdateInputs()
    {
        PlayerInteract();       // 상호작용 물체 탐색
        PlayerAttack();
        PlayerJump();
    }

    private void Update()
    {
        playerCam.Rotate();     // 카메라 회전
        PlayerMove();           // 플레이어 이동
        UpdateInputs();         // 기타 조작
    }

    public override void Start()
    {
        base.Start();

        PlayManager.SetCurPlayer(this);
        PlayManager.SetPlayerMaxHP(MaxHP);

        playerRB = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<PlayerCamera>();
    }
}
