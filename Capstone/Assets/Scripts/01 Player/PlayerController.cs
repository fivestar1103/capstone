using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public partial class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerHP = 100;
    public float PlayerHP { get { return playerHP; } }

    private Rigidbody rb;        // 2D Rigidbody 사용 (3D라면 Rigidbody 사용)

    // 입력 관련 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }  // Input System Player 입력                                                          

    private void Update()
    {
        PlayerMove();
        PlayerRotate();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
