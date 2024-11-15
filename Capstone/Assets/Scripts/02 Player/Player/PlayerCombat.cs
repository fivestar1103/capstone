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
    private float duration = 10f;

    private PlayerAttack preparedSkill;   // 주문을 말한 후 생성된 스킬 오브젝트

    public ESkill CurSkill { get; private set; }

    public bool IsBuffApplied { get; set; }

    public void PlayerAttack()
    {
        if (AttackTrigger)
        {
            // TempAttack();
            UseSkill();
        }
    }


    private void TempAttack()
    {
        GameObject bullet = Instantiate(attackObject, attackPos.position, attackPos.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = attackPos.forward * AttackSpeed;
        }
    }

    // 현재 구현 중인 부분
    private void PrepareSkill(string _spell, EEmotion _emotion)
    {
        for (int i = 0; i < (int)ESkill.LAST; i++)
        {
            if (_spell == GameManager.Skills[i].Spell) CurSkill = (ESkill)i;
            GameManager.Skills[(int)CurSkill].SetSkill(CurSkill, _emotion);
        }
        preparedSkill = Instantiate(GameManager.Skills[(int)CurSkill], attackPos.position, attackPos.rotation, attackPos);

        /* 생성된 스킬 오브젝트를 보정하는 부분 */
        preparedSkill.transform.localPosition = new Vector3(0, 0, 0.5f); // 약간 앞쪽으로 위치 보정
        preparedSkill.transform.localRotation = Quaternion.identity;
    }

    private void UseSkill()
    {
        Rigidbody rb = preparedSkill.GetComponent<Rigidbody>();
        if (rb != null)
        {
            preparedSkill.transform.SetParent(null);
            rb.isKinematic = false;
            rb.velocity = attackPos.forward * AttackSpeed;
        }
    }
    //

    IEnumerator ApplyBuff(EEmotion _emotion)
    {
        float elapsedTime = 0f;

        if (_emotion == EEmotion.EHappy)        // 공격력 증가
        {
            Attack *= 1.5f;
        }
        else if (_emotion == EEmotion.ENeutral) // 공속 증가
        {
            AttackSpeed *= 1.5f;
        }
        else if (_emotion == EEmotion.EAngry)   // 피흡
        {
            
        }
        
        IsBuffApplied = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void SetOriginalStat(EEmotion _emotion)
    {
        switch (_emotion)
        {
            case EEmotion.EHappy:
                Attack /= 1.5f;
                break;
            case EEmotion.ENeutral:
                AttackSpeed /= 1.5f;
                break;
            case EEmotion.EAngry:
                // IsDrained flag sets to false
                break;
            default:
                return;
        }
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

    public void Drain(float _hp)
    {
        if (curHP > 0)
        {
            curHP += _hp;
            if (curHP >= PlayManager.MaxHP)
            {
                curHP = PlayManager.MaxHP;
            }
        }
    }
}
