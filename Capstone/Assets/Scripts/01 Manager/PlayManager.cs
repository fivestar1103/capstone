using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager Inst;

    // 게임 시작 관련 함수
    private void StartPlay()
    {
        GameManager.SetControlMode(EControlMode.FIRST_PERSON);
    }

    // 플레이어
    private static PlayerController Player { get; set; }
    public static float MaxHP { get { return Player.MaxHP; } }                                                                              // 플레이어 최대 체력
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // 플레이어 등록
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // 플레이어인지 확인
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefinition.NULL_VECTOR; } }      // 플레이어 위치
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefinition.NULL_VECTOR; } }              // 플레이어 평면 위치
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // 플레이어 등록 여부
    public static float GetDistToPlayer(Vector2 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos2 - _pos).magnitude; }                // 플레이어와의 거리
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // 상호작용 종료
    public static void StopPlayerInteract(InteractScript _interact) { Player.StopInteract(_interact); }


    // 몬스터 관련
    public static MonsterSpawnPoint[] spawnPoints;      // 몬스터 스폰 포인트
    public static int huntedMonsterNum = 0;             // 사냥당한 몬스터 수

    // UI
    private static UIManager UI;
    public static bool IsDialogueOpened { get { return UI.IsDialogueOpened; } } // 대화창 열렸는지
    public static void OpenDialogue(NPCScript _npc) { UI.OpenDialogue(_npc); }    // 대화창 열기
    public static void CloseDialogue() { UI.CloseDialogue(); }  // 대화창 닫기

    public static void ToggleSupporterUI(bool _toggle) { UI.ToggleSupporterUI(_toggle); }
    public static void SetHPInfo(float _curHP) { UI.SetHPInfo(_curHP); }        // 체력바 설정


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

    private void Start()
    {
        StartPlay();
    }
}
