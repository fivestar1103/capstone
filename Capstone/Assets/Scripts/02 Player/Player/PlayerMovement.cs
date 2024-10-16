using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public partial class PlayerController
{
    [SerializeField]
    private float moveSpeed = 5f;         // 움직임 속도
    [SerializeField]
    private float jumpHeight = 3f;        // 점프 세기
    [SerializeField]
    private float mouseSensitivity = 100; // 마우스 감도
    [SerializeField]
    private float groundDistance = 0.1f;

    public Vector3 MoveInput { get; private set; }     // wasd 입력
    public Vector2 MouseDelta { get; private set; }    // 마우스 좌표
    public bool IsGround { get; private set; }

    // PlayerInput 컴포넌트에서 Move 액션에 대한 입력을 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 입력을 받음; 일단은 무한 점프인데 수정 예정
        if (context.performed)
        {
            CheckGround();
            if (IsGround)
            {
                Vector3 jumpForce = Vector3.up * Mathf.Sqrt(jumpHeight * -Physics.gravity.y);
                playerRB.AddForce(jumpForce, ForceMode.VelocityChange); // 위로 점프
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    private void PlayerMove()
    {
        Vector3 moveDirection = transform.forward * MoveInput.y + transform.right * MoveInput.x;
        Vector3 moveVelocity = moveDirection * moveSpeed;

        playerRB.velocity = new Vector3(moveVelocity.x, playerRB.velocity.y, moveVelocity.z);
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance, ValueDefinition.GROUND_LAYER))
            IsGround = true;
        else
            IsGround = false;
    }
}
