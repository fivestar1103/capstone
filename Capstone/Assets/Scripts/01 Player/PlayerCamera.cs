using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 10f; // 마우스 감도
    [SerializeField]
    private Transform player;             // 플레이어의 몸체

    private float xRotation = 0f;          
    private Vector2 mouseDelta;            

    // 마우스 움직임 입력 처리 (Look)
    public void OnLook(InputAction.CallbackContext context)
    {
        // 마우스 움직임을 받아 mouseDelta에 저장
        mouseDelta = context.ReadValue<Vector2>();
        Rotate();
    }

    private void Rotate()
    {
        // 마우스 입력 값 (델타) 가져오기
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // --- 카메라 상하 회전 (Pitch) ---
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 상하 회전을 -90도에서 90도로 제한

        // 카메라 상하 회전 적용 (X축 회전)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // --- 플레이어 좌우 회전 (Yaw) ---
        player.Rotate(Vector3.up * mouseX); // 플레이어 본체를 Y축 기준으로 회전
        transform.Rotate(Vector3.up * mouseX);
    }
}
