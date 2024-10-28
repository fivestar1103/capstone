using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class MonsterScript
{
    public enum EMonsterState
    {
        WANDER,
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    [SerializeField]
    private float wanderRadius = 4.0f;
    [SerializeField]
    private float attackDist = 2.0f;
    [SerializeField]
    private float traceDist = 10.0f;

    private EMonsterState state = EMonsterState.WANDER;
    private bool isDie = false;

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            if (state == EMonsterState.DIE) yield break;

            float distance = Vector3.Distance(PlayManager.PlayerPos, transform.position);

            if (distance <= attackDist)
            {
                state = EMonsterState.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = EMonsterState.TRACE;
            }
            else
                state = EMonsterState.WANDER;
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                case EMonsterState.WANDER:
                    if (!monsterNav.hasPath || monsterNav.remainingDistance < 0.1f)
                    {
                        Vector3 randomPos = RandomNavSphere(transform.position, wanderRadius, 1 << 0);  // Walkable 영역에서만 적용되게 함
                        monsterNav.SetDestination(randomPos);
                        monsterNav.isStopped = false;
                    }
                    break;
                case EMonsterState.TRACE:
                    monsterNav.SetDestination(PlayManager.PlayerPos);
                    monsterNav.isStopped = false;
                    break;
                case EMonsterState.ATTACK:
                    // 공격
                    break;
                case EMonsterState.DIE:
                    monsterNav.isStopped = true;
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;  // 반경 내 랜덤한 방향 설정
        randomDirection += origin;  // 현재 위치 기준으로 오프셋 추가

        NavMeshHit navHit;
        // NavMesh 위의 유효한 위치 찾기
        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;  // 유효한 위치 반환
    }

    private void OnEnable()
    {
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }
}
