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

    public void Die()
    {
        if (isDead)
            return;
        isDead = true;

        OnPlayerDeath?.Invoke();
        // 사망 애니메이션
        // 기타 사망 로직
    }


    private void Update()
    {
        playerCam.Rotate();
        PlayerMove();
        DetectObject();
    }

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<PlayerCamera>();
    }
}
