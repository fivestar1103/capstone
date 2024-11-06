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
    private float groundDistance = 1.5f;

    public bool IsGround { get; private set; }

    public void OnJump()
    {
        if (JumpTrigger)
        {
            CheckGround();
            if (IsGround)
            {
                Vector3 jumpForce = Vector3.up * Mathf.Sqrt(jumpHeight * -Physics.gravity.y);
                playerRB.AddForce(jumpForce, ForceMode.VelocityChange); // 위로 점프
            }
        }
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
        IsGround = Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance, ValueDefinition.GROUND_LAYER);
    }
}
