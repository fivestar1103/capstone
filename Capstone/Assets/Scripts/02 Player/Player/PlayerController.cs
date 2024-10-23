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

    public event Action OnPlayerDeath;

    private bool isDead = false;

    // 입력 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }  // Input System Player 입력
    public Vector3 MoveInput { get { return PlayerInput.Movement.ReadValue<Vector2>(); } }      // wasd 입력
    public Vector2 MouseDelta { get { return PlayerInput.Look.ReadValue<Vector2>(); } }         // 마우스 좌표
    public bool AttackTrigger { get; private set; }                    // 공격
    public bool JumpTrigger { get; private set; }                      // 점프                                                                                
    public bool InteractTrigger { get { return PlayerInput.Interact.triggered; } }              // 상호작용
    public bool SupportUITrigger { get { return PlayerInput.SupportUI.triggered; } }            // AI 대화창


    public void Die()
    {
        if (isDead)
            return;
        isDead = true;

        OnPlayerDeath?.Invoke();
        // 사망 애니메이션
        // 기타 사망 로직
    }

    private void UpdateInputs()
    {
        AttackTrigger = PlayerInput.Attack.IsPressed();
        JumpTrigger = PlayerInput.Jump.IsPressed();
    }

    private void Update()
    {
        PlayerInteract();       // 상호작용 물체 탐색

        playerCam.Rotate();     // 카메라 회전
        PlayerMove();           // 플레이어 이동

        UpdateInputs();         // 기타 조작
    }

    private void Start()
    {
        PlayManager.SetCurPlayer(this);
        playerRB = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<PlayerCamera>();
    }
}
