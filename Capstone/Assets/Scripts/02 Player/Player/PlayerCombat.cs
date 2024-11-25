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
    private float buffDuration = 10f;

    private PlayerAttack preparedSkill;   // 주문을 말한 후 생성된 스킬 오브젝트

    public ESkill CurSkill { get; private set; }

    private bool canAttack = true;
    public bool IsBuffApplied { get; private set; }
    public bool IsDrain { get; private set; }

    public void PlayerAttack()
    {
        if (AttackTrigger && canAttack)
        {
            // Test Calling
            PrepareSkill(ValueDefinition.SPELL1, EEmotion.ENeutral);
            StartCoroutine(UseSkill());
        }
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

    IEnumerator UseSkill()
    {
        canAttack = false;

        Rigidbody rb = preparedSkill.GetComponent<Rigidbody>();
        if (rb != null)
        {
            preparedSkill.transform.SetParent(null);
            rb.isKinematic = false;
            rb.velocity = attackPos.forward * 10;   // 수치 조정 필요
        }
        if (((int)preparedSkill.StatusEffect >= 0 && (int)preparedSkill.StatusEffect <= 2) && !IsBuffApplied) 
            StartCoroutine(ApplyBuff(preparedSkill.StatusEffect));

        yield return new WaitForSeconds(AttackSpeed);
        canAttack = true;
    }

    IEnumerator ApplyBuff(EStatusEffect _buff)
    {
        IsBuffApplied = true;
        float elapsedTime = 0f;

        if (_buff == EStatusEffect.ATTACK_UP)        // 공격력 증가
        {
            Attack *= 1.5f;
            
        }
        else if (_buff == EStatusEffect.ATTACK_SPEED_UP) // 공속 증가(== 공격 쿨타임 감소)
        {
            AttackSpeed /= 1.5f;
           
        }
        else if (_buff == EStatusEffect.DRAIN)   // 피흡
        {
            IsDrain = true;
        }

        while (elapsedTime < buffDuration)
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
                AttackSpeed *= 1.5f;
                break;
            case EStatusEffect.DRAIN:
                IsDrain = false;
                break;
            default:
                return;
        }
        IsBuffApplied = false;
    }

    public override void GetHit(float _damage)
    {
        curHP -= _damage;
        PlayManager.SetPlayerCurHP(curHP);

        if (curHP <= 0 && !isDead)
        {
            PlayerDie(); 
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
            PlayManager.SetPlayerCurHP(curHP);
        }
    }
}
