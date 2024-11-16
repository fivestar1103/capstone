using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MonsterScript
{
    [SerializeField]
    private float duration = 3f; // cc기 지속시간

    public bool IsDotted { get; private set; }  // 도트 데미지 여부

    public override void GetHit(float _damage)
    {
        curHP -= _damage;
        Debug.Log(curHP);

        if (curHP <= 0)
        {
            Destroy(this.gameObject);
            PlayManager.huntedMonsterNum++;
            // Etc.
        }
    }

    IEnumerator TempAttack()
    {
        this.transform.LookAt(PlayManager.PlayerPos);

        IsAttack = true;

        GameObject bullet = Instantiate(monsterBullet, attackPoint.position, attackPoint.rotation);

        MonsterAttack monsterAttack = bullet.GetComponent<MonsterAttack>();
        if (monsterAttack != null) monsterAttack.attack = this.Attack;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null) rb.velocity = attackPoint.forward * 20;

        yield return new WaitForSeconds(1.0f);

        IsAttack = false;
    }

    // 지속시간 동안 상태이상 적용
    IEnumerator ApplyCCType(EStatusEffect _ccType)
    {
        if(_ccType == EStatusEffect.SLOW) // 둔화
        {
            this.Speed *= 0.5f;
        }
        else if (_ccType == EStatusEffect.DOT_DAMAGE) // 도트뎀
        {
            IsDotted = true;
            StartCoroutine(DottedDamage());
        }
        else if (_ccType == EStatusEffect.STUN) // 스턴
        {
            this.monsterNav.isStopped = true;
            StopCoroutine(TempAttack());
            IsAttack = false;
        }
        else if(_ccType == EStatusEffect.NERF_STAT)
        {
            this.Attack *= 0.7f;
            this.Defense *= 0.7f;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RemoveCC(_ccType);
    }

    // 스탯 원상 복구
    private void RemoveCC(EStatusEffect _ccType)
    {
        switch(_ccType)
        {
            case EStatusEffect.SLOW:
                this.Speed /= 0.5f;
                break;
            case EStatusEffect.DOT_DAMAGE:
                IsDotted = false;
                break;
            case EStatusEffect.STUN:
                this.monsterNav.isStopped = false;
                StartCoroutine(TempAttack());
                IsAttack = true;
                break;
            case EStatusEffect.NERF_STAT:
                this.Attack /= 0.7f;
                this.Defense /= 0.7f;
                break;
            default:
                return;
        }
    }

    IEnumerator DottedDamage()
    {
        while(IsDotted)
        {
            curHP -= 5f;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
