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
            GetHit(PlayManager.PlayerAttack);
            // 여기서 폭발 파티클
            if (playerAttack != null) StartCoroutine(ApplyCCType(playerAttack.ccType));    // 감정에 의한 CC기 적용
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
