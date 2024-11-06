using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public partial class MonsterScript : ObjectScript
{
    private NavMeshAgent monsterNav;

    public override void GetHit(float _damage)
    {
        curHP -= _damage;
        if (curHP <= 0)
        {
            Destroy(this.gameObject);
            PlayManager.huntedMonsterNum++;
            // Etc.
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
