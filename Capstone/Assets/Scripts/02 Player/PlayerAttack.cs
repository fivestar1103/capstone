using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private float lifeTime = 1.5f;

    private void OnCollisionEnter(Collision _other)
    {
        if(_other.gameObject.CompareTag(ValueDefinition.MONSTER_TAG))
        {
            MonsterScript monster = _other.gameObject.GetComponent<MonsterScript>();

            if(monster != null)
            {
                monster.GetHit(PlayManager.PlayerAttack);
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
