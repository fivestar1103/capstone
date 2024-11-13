using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerController
{
    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private GameObject attackObject;
    [SerializeField]
    private float attackSpeed = 100f;

    public PlayerAttack[] skills;

    public void PlayerAttack()
    {
        if(AttackTrigger)
        {
            TempAttack();
        }
    }

    // ÀÓ½Ã °ø°Ý ·ÎÁ÷ -> ÅºÈ¯ ¹ß»ç ´À³¦
    private void TempAttack()
    {
        GameObject bullet = Instantiate(attackObject, attackPos.position, attackPos.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = attackPos.forward * attackSpeed;  
        }
    }

    private void RealAttack(string _spell)
    {
        GameObject bullet = Instantiate(attackObject, attackPos.position, attackPos.rotation);

    }

    public override void GetHit(float _damage)
    {
        curHP -= _damage;
        PlayManager.SetPlayerCurHP(curHP);

        if (curHP < 0)
        {
            // »ç¸Á ·ÎÁ÷
            // Etc.
        }
    }
}
