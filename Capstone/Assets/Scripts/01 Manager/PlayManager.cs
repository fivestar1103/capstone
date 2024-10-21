using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Inst;

    // 플레이어
    private static PlayerController Player { get; set; }
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // 플레이어 등록
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // 플레이어인지 확인
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefinition.NULL_VECTOR; } }      // 플레이어 위치
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefinition.NULL_VECTOR; } }              // 플레이어 평면 위치
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // 플레이어 등록 여부
    public static float GetDistToPlayer(Vector2 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos2 - _pos).magnitude; }                // 플레이어와의 거리

    // UI
    private static UIManager UI;
    public static void OpenDialogue() { UI.OpenDialogue(); }    // 대화창 열기
    public static void CloseDialogue() { UI.CloseDialogue(); }  // 대화창 닫기


    private void SetSubManagers()
    {
        UI = GetComponent<UIManager>();
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(Inst.gameObject); }
        Inst = this;
        SetSubManagers();
    }
}
