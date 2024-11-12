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

    IEnumerator ApplyCCType(ECCType _ccType)
    {
        float elapsedTime = 0f;

        if(_ccType == ECCType.ECCType1) // 둔화
        {
            this.CurSpeed *= 0.5f;
        }
        else if (_ccType == ECCType.ECCType2) // 도트뎀
        {
            IsDotted = true;
            StartCoroutine(DottedDamage());
        }
        else if (_ccType == ECCType.ECCType3) // 스턴
        {
            this.monsterNav.isStopped = true;
            IsAttack = false;
        }
        // 추후 추가...

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void RemoveCC(ECCType _ccType)
    {
        switch(_ccType)
        {
            case ECCType.ECCType1:
                this.CurSpeed /= 0.5f;
                break;
            case ECCType.ECCType2:
                IsDotted = false;
                break;
            case ECCType.ECCType3:
                this.monsterNav.isStopped = false;
                break;
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
