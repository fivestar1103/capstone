using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class MonsterScript : ObjectScript
{
    [SerializeField]
    private GameObject monsterBullet;
    [SerializeField]
    private Transform attackPoint;

    private NavMeshAgent monsterNav;
    private MonsterHPbar HPbar;
    public float MonsterAttack { get { return Attack; } }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.gameObject.CompareTag(ValueDefinition.PLAYER_ATTACK_TAG))
        {
            PlayerAttack playerAttack = _other.gameObject.GetComponent<PlayerAttack>();

            GetHit(PlayManager.PlayerAttack);   // 피흡량 조절 필요
            if (playerAttack != null)
            {
                if (PlayManager.IsDrain)
                {
                    PlayManager.Drain(PlayManager.PlayerAttack);
                }
                if ((int)playerAttack.StatusEffect >= 3 && (int)playerAttack.StatusEffect <= 6 && !IsDebuffed) // 상태이상인 경우
                {
                    StartCoroutine(ApplyCCType(playerAttack.StatusEffect));
                }
            }
        }
    }

    private void Update()
    {
        CheckMonsterState();
        MonsterAction();
    }

    private void Awake()
    {
        HPbar = GetComponentInChildren<MonsterHPbar>();

        monsterNav = GetComponent<NavMeshAgent>();
        monsterNav.speed = Speed;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GameManager.RegisterMonster(this);

        IsDie = false;
        HPbar.SetComps();   // 활성화 될 때마다 다시 MaxHP로
        monsterNav.isStopped = false;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnDisable()
    {
        GameManager.UnregisterMonster(this);
    }
}
