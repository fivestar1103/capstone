using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public partial class PlayerController
{
    private void OnTriggerEnter(Collider _other)
    {
        switch(_other.tag)
        {
            case ValueDefinition.MONSTER_ATTACK_TAG:    // 몬스터 공격에 피격
                GetHit(_other);
                break;
        }
      
    }
}
