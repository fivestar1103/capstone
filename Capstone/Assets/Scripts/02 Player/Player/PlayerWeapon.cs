using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private PlayerController player;
    public PlayerController Player { get { return player; } }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag(ValueDefinition.MONSTER_TAG))
        {
            _other.transform.GetComponentInChildren<MonsterScript>()?.GetHit(this.gameObject);
        }
    }

    private void Awake()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }
}
