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
    public float MonsterAttack { get { return Attack; } }

    private void OnCollisionEnter(Collision _other)
    {
        if (_other.gameObject.CompareTag(ValueDefinition.PLAYER_ATTACK_TAG))
        {
            PlayerAttack playerAttack = _other.gameObject.GetComponent<PlayerAttack>();
            GetHit(PlayManager.PlayerAttack);   // 피흡량 조절 필요
            if (playerAttack != null)
            {
                if(PlayManager.IsDrain)
                {
                    playerAttack.Drain(PlayManager.PlayerAttack);
                }
                if ((int)playerAttack.StatusEffect >= 3 && (int)playerAttack.StatusEffect <= 6) // 상태이상인 경우
                {
                    StartCoroutine(ApplyCCType(playerAttack.StatusEffect));    
                }
            }
            Destroy(_other.gameObject);
        }
    }

    private void Update()
    {
        if(!IsDie)
        {
            CheckMonsterState();
            MonsterAction();
        }
    }

    public override void Start()
    {
        base.Start();

    }

    private void Awake()
    {
        monsterNav = GetComponent<NavMeshAgent>();
    }
}
