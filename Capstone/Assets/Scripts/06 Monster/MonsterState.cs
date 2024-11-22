using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class MonsterScript
{
    public enum EMonsterState
    {
        PAUSED,
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    [SerializeField]
    private float PATROLRadius = 5.0f;
    [SerializeField]
    private float attackDist = 10.0f;
    [SerializeField]
    private float traceDist = 20.0f;

    private EMonsterState state = EMonsterState.PATROL;

    public bool IsAttack { get; private set; }
    public bool IsDie { get; private set; }

    private void CheckMonsterState()
    {
        float distance = Vector3.Distance(PlayManager.PlayerPos, transform.position);

        if (GameManager.ControlMode == EControlMode.UI_CONTROL)
            state = EMonsterState.PAUSED;
        else if(GameManager.ControlMode == EControlMode.FIRST_PERSON)
        {
            if (distance <= attackDist)
                state = EMonsterState.ATTACK;
            else if (distance <= traceDist)
                state = EMonsterState.TRACE;
            else if (curHP <= 0)
                state = EMonsterState.DIE;
            else
                state = EMonsterState.PATROL;
        }
    }

    private void MonsterAction()
    {
        switch (state)
        {
            case EMonsterState.PAUSED:
                return;
            case EMonsterState.PATROL:
                if (!monsterNav.hasPath || monsterNav.remainingDistance < 0.1f)
                {
                    Vector3 randomPos = RandomNavSphere(transform.position, PATROLRadius, 1 << 0);  // Walkable 영역에서만 적용되게 함
                    monsterNav.SetDestination(randomPos);
                    monsterNav.isStopped = false;
                }
                break;
            case EMonsterState.TRACE:
                monsterNav.SetDestination(PlayManager.PlayerPos);
                monsterNav.isStopped = false;
                break;
            case EMonsterState.ATTACK:
                if(!IsAttack) StartCoroutine(TempAttack());
                break;
            case EMonsterState.DIE:
                IsDie = true;
                monsterNav.isStopped = true;
                GetComponent<CapsuleCollider>().enabled = false;
                break;
        }
    }

    private Vector3 RandomNavSphere(Vector3 _origin, float _distance, int _layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * _distance;  // 반경 내 랜덤한 방향 설정
        randomDirection += _origin;  // 현재 위치 기준으로 오프셋 추가

        NavMeshHit navHit;
        // NavMesh 위의 유효한 위치 찾기
        NavMesh.SamplePosition(randomDirection, out navHit, _distance, _layermask);

        return navHit.position;  // 유효한 위치 반환
    }
}
