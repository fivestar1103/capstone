using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Attack");
        }
    }

    public override void GetHit(GameObject _hit)
    {   
        // 몬스터의 공격에만 피격이 적용되도록 함
        MonsterScript monsterHit = _hit.GetComponent<MonsterScript>();

        if(monsterHit != null)
        {
            CurHP -= monsterHit.Attack;
            if(CurHP < 0)
            {
                Die();
            }
        }
    }
}
