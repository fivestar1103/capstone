using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    public static float PlayerAttack { get { return Player.Attack; } }                                                                      // 플레이어 공격력
    public static void SetCurPlayer(PlayerController _player) { Player = _player; }                                                         // 플레이어 등록
    public static bool CheckIsPlayer(ObjectScript _object) { return _object == Player; }                                                    // 플레이어인지 확인
    public static Vector3 PlayerPos { get { if (IsPlayerSet) return Player.transform.position; return ValueDefinition.NULL_VECTOR; } }      // 플레이어 위치
    public static Vector2 PlayerPos2 { get { if (IsPlayerSet) return Player.Position2; return ValueDefinition.NULL_VECTOR; } }              // 플레이어 평면 위치
    public static bool IsPlayerSet { get { return Player != null; } }                                                                       // 플레이어 등록 여부
    public static float GetDistToPlayer(Vector2 _pos) { if (!IsPlayerSet) return -1; return (PlayerPos2 - _pos).magnitude; }                // 플레이어와의 거리
    public static void StopPlayerInteract() { Player.StopInteract(); }                                                                      // 상호작용 종료
    public static void StopPlayerInteract(InteractScript _interact) { Player.StopInteract(_interact); }

    // 전투 관련
    
    public static bool IsDrain { get { return Player.IsDrain; } }
    public static void Drain(float _hp) { Player.Drain(_hp); }

    // 몬스터 관련
    public static MonsterSpawnPoint[] spawnPoints;      // 몬스터 스폰 포인트
    public static int TotalMonsterNum { get { return GameManager.ObjectNum; } }             // 사냥해야 하는 몬스터 수
    public static int CurMonsterNum;                    // 현재 소환된 몬스터 수
    public static int MonsterNum;                       // 사냥당한 몬스터 수

    // UI
    private UIManager playerUI;
    private static UIManager UIManager { get { return Inst.playerUI; } }
    public static bool IsDialogueOpened { get { return UIManager.IsDialogueOpened; } }   // 대화창 열렸는지
    public static void OpenDialogue(NPCScript _npc) { UIManager.OpenDialogue(_npc); }    // 대화창 열기
    public static void CloseDialogue() { UIManager.CloseDialogue(); }                    // 대화창 닫기
    public static void SetMonsterNum() { UIManager.SetMonsterNum(); }                    // 전투 시 몬스터 수 정보 

    public static void ToggleSupporterUI(bool _toggle) { UIManager.ToggleSupporterUI(_toggle); }
    public static void SetPlayerMaxHP(float _hp) { UIManager.SetMaxHP(_hp); }    // 체력바 최대 체력
    public static void SetPlayerCurHP(float _hp) { UIManager.SetCurHP(_hp); }    // 체력바 현재 체력


    private void SetSubManagers()
    {
        playerUI = GetComponent<UIManager>();
        playerUI.SetManager();
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
