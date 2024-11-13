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
            // ¿©±â¼­ Æø¹ß ÆÄÆ¼Å¬
            if (playerAttack != null) StartCoroutine(ApplyCCType(playerAttack.ccType));    // °¨Á¤¿¡ ÀÇÇÑ CC±â Àû¿ë
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
