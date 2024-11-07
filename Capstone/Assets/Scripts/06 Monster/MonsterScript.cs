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
            GetHit(PlayManager.PlayerAttack);
            // 여기서 폭발 파티클
            Destroy(_other.gameObject);
        }
    }

    public override void GetHit(float _damage)
    {
        curHP -= _damage;
        Debug.Log(curHP);
        if (curHP <= 0)
        {
            Destroy(this.gameObject);
            PlayManager.huntedMonsterNum++;
            // Etc.
        }
    }

    IEnumerator TempAttack()
    {
        this.transform.LookAt(PlayManager.PlayerPos);

        IsAttack = true;

        GameObject bullet = Instantiate(monsterBullet, attackPoint.position, attackPoint.rotation);

        MonsterAttack monsterAttack = bullet.GetComponent<MonsterAttack>();
        if (monsterAttack != null) monsterAttack.attack = this.Attack;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = attackPoint.forward * 20;
        
        yield return new WaitForSeconds(1.0f);

        IsAttack = false;
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
