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

    private Vector3 moveInput;   // 입력 값 저장
    private Rigidbody rb;        // 2D Rigidbody 사용 (3D라면 Rigidbody 사용)

    // 입력 관련 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }                      // Input System Player 입력
    public Vector2 InputVector { get { return PlayerInput.Movement.ReadValue<Vector2>(); } }                        // WASD 방향
    public bool JumpPressing { get; private set; }                                                                  // 스페이스바

    // PlayerInput 컴포넌트에서 Move 액션에 대한 입력을 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

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
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 moveVelocity = moveDirection * moveSpeed;
        
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z); // y축 속도는 유지
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
