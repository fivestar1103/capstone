using System;
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

    public bool IsDrain { get; private set; }

    public void PlayerAttack()
    {
        if (AttackTrigger) 
            UseSkill();
    }

    // 현재 구현 중인 부분
    private void PrepareSkill(string _spell, EEmotion _emotion)
    {
        int idx;
        for (idx = 0; idx < (int)ESkill.LAST; idx++)
        {
            if (_spell == GameManager.Skills[idx].Spell)
            {
                CurSkill = (ESkill)idx;
                break;
            }
        }
        preparedSkill = Instantiate(GameManager.Skills[(int)CurSkill], attackPos.position, attackPos.rotation, attackPos);
        preparedSkill.SetSkill(CurSkill, _emotion);

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

        if ((int)preparedSkill.StatusEffect >= 0 && (int)preparedSkill.StatusEffect <= 2) // 버프인 경우
            StartCoroutine(ApplyBuff(preparedSkill.StatusEffect));
    }

    IEnumerator ApplyBuff(EStatusEffect _buff)
    {
        float elapsedTime = 0f;

        if (_buff == EStatusEffect.ATTACK_UP)        // 공격력 증가
        {
            Attack *= 1.5f;
            
        }
        else if (_buff == EStatusEffect.ATTACK_SPEED_UP) // 공속 증가
        {
            AttackSpeed *= 1.5f;
           
        }
        else if (_buff == EStatusEffect.DRAIN)   // 피흡
        {
            IsDrain = true;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetOriginalStat(_buff);
    }

    private void SetOriginalStat(EStatusEffect _buff)
    {
        switch (_buff)
        {
            case EStatusEffect.ATTACK_UP:
                Attack /= 1.5f;
                break;
            case EStatusEffect.ATTACK_SPEED_UP:
                AttackSpeed /= 1.5f;
                break;
            case EStatusEffect.DRAIN:
                IsDrain = false;
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
