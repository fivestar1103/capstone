using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;         // 움직임 속도
    [SerializeField]
    private float jumpForce = 10f;        // 점프 세기

    private Vector3 moveInput;   // wasd 입력
    private Vector2 mouseDelta;  // 마우스 회전값
    private Rigidbody rb;        // 2D Rigidbody 사용 (3D라면 Rigidbody 사용)

    [SerializeField]
    private float playerHP = 100;
    public float PlayerHP { get { return playerHP; } }

    // 입력 관련 관련
    private PlayerInput.PlayerActions PlayerInput { get { return GameManager.PlayerInputs; } }                      // Input System Player 입력
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

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void PlayerMove()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 moveVelocity = moveDirection * moveSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
