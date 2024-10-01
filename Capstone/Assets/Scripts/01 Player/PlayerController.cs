using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public partial class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // 움직임 속도
    [SerializeField]
    private float jumpForce = 10f; // 점프 세기
    
    private Vector3 moveInput;    // wsad 값

    private Rigidbody rb;       

    // 입력 관련 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }                      // Input System Player 입력
    public bool JumpPressing { get; private set; }                                                                  // 스페이스바

    // wsad 이동
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // 점프
    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 입력을 받음; 일단은 무한 점프인데 수정 예정
        if (context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 위로 점프
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // 플레이어의 카메라가 바라보는 방향을 기준으로 이동 벡터를 계산
        Vector3 forward = transform.forward; // 플레이어가 바라보는 방향의 forward (Z축)
        Vector3 right = transform.right;     // 플레이어가 바라보는 방향의 right (X축)

        // 방향키 입력에 따른 이동 방향 계산
        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // 플레이어를 이동시킴
        Vector3 moveVelocity = moveDirection * moveSpeed;

        // Rigidbody를 사용해 이동 (y축 고정)
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
