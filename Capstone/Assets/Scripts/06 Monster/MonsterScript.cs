using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public partial class MonsterScript : ObjectScript
{
    private NavMeshAgent monsterNav;

    private void OnTriggerEnter(Collider _other)
    {
        switch (_other.tag)
        {
            case ValueDefinition.PLAYER_WEAPON_TAG :    // 플레이어 공격에 피격
                GetHit(_other.gameObject);
                break;
        }
    }

    public override void GetHit(GameObject _hit)
    {
        // 플레이어의 공격에만 피격이 적용되도록 함
        PlayerWeapon PlayerHit = _hit.GetComponent<PlayerWeapon>();

        if (PlayerHit != null)
        {
            curHP -= PlayerHit.Player.Attack;
            if (curHP < 0)
            {
                Destroy(gameObject);
                PlayManager.huntedMonsterNum++;
                // Etc.
            }
        }
    }

    private void Awake()
    {
        monsterNav = GetComponent<NavMeshAgent>();
    }
}
