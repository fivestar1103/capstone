using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    [SerializeField]
    private float moveSpeed = 5f;         // 움직임 속도
    [SerializeField]
    private float jumpForce = 10f;        // 점프 세기
    [SerializeField]
    private float mouseSensitivity = 100; // 마우스 감도

    private Vector3 moveInput;   // wasd 입력
    private Vector2 mouseDelta;  // 마우스 회전값

    private float xRotation = 0f; // X축 회전
    private CinemachineVirtualCamera playerCam;

    private bool IsGrounded = true;

    // PlayerInput 컴포넌트에서 Move 액션에 대한 입력을 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 입력을 받음; 일단은 무한 점프인데 수정 예정
        if (context.performed && IsGrounded)
        {
            IsGrounded = false;
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

    private void PlayerRotate()
    {
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 카메라 상하 회전 각도 제한

        // 카메라의 로컬 회전 설정
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
