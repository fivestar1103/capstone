using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    // 입력
    private InputManager inputManager;
    public static InputManager InputManager { get { return Inst.inputManager; } }
    public static PlayerInput.PlayerActions PlayerInputs { get { return InputManager.PlayerInputs; } }                                      // 플레이어 Input
    public static PlayerInput.UIControlActions UIControlInputs { get { return InputManager.UIControlInputs; } }                             // UI조작 Input
    public static EControlMode ControlMode { get { return InputManager.CurControlMode; } }                                                  // 조작 모드
    public static float MouseSensitive { get { return InputManager.MouseSensitive; } }                                                      // 마우스 민감도
    public static void SetControlMode(EControlMode _mode) { InputManager.SetControlMode(_mode); }                                           // 조작 모드 변경
    public static void SetMouseSensitive(float _sensitive) { InputManager.SetMouseSensitive(_sensitive); }                                  // 마우스 민감도 설정

    // 스킬
    private SkillManager skillManager;
    public static SkillManager SkillManager { get { return Inst.skillManager; } }
    public static PlayerAttack[] Skills { get { return SkillManager.Skills; } }

    // 전투
    private List<MonsterScript> monsters = new List<MonsterScript>();
    private void HandlePlayerDeath()    // 플레이어가 죽었을 때 시스템에서 처리할 기능
    {
        Debug.Log("Death!!");
        NotifyMonsters();
    }
    public void RegisterMonster(MonsterScript monster)
    {
        if (!monsters.Contains(monster))
        {
            monsters.Add(monster);
            Debug.Log($"{monster.name} registered to GameManager.");
        }
    }
    public void UnregisterMonster(MonsterScript monster)
    {
        if (monsters.Contains(monster))
        {
            monsters.Remove(monster);
            Debug.Log($"{monster.name} unregistered from GameManager.");
        }
    }
    public void NotifyMonsters()    // 플레이어가 죽었을 때 몬스터 단에서 처리할 기능
    {
        Debug.Log("Notifying all monsters about player's death...");
        foreach (var monster in monsters)
        {
            monster.ReactToPlayerDeath();
        }
    }

    private void SetSubManagers()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.SetManager();
        skillManager = GetComponent<SkillManager>();
        skillManager.SetManager();
    }

    private void Awake()
    {
        if (Inst != null) { Destroy(gameObject); return; }
        Inst = this;
        DontDestroyOnLoad(gameObject);
        SetSubManagers();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }
}
