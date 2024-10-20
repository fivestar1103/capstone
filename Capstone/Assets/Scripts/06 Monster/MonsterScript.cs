using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : ObjectScript
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
            CurHP -= PlayerHit.Player.Attack;
            if (CurHP < 0)
            {
                Destroy(gameObject);
                // Etc.
            }
        }
    }
}
