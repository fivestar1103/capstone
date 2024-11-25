using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private MonsterScript monster;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private float spawnRadius;

    IEnumerator MonsterSpawn()
    {
        while (PlayManager.CurMonsterNum < PlayManager.TotalMonsterNum)
        {
            Vector3 spawnPosition = GetRandomPositionWithinRadius();

            // int idx = Random.Range(0, monster.Length);
            // MonsterScript mob = monster[idx];

            // Instantiate(mob, transform.position, Quaternion.identity);

            //Instantiate(monster, spawnPosition, Quaternion.identity);

            GameObject monster = GameManager.GetPooledObject();
            if (monster != null)
            {
                monster.transform.position = spawnPosition;
                monster.transform.rotation = Quaternion.identity;
                monster.SetActive(true);

                PlayManager.CurMonsterNum++;
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    Vector3 GetRandomPositionWithinRadius()
    {
        // 2D 상에서 반경 내의 랜덤한 위치 생성 (x, z 축 사용)
        Vector2 randomPos2D = Random.insideUnitCircle * spawnRadius;
        Vector3 randomPos = new Vector3(randomPos2D.x, 0, randomPos2D.y);

        return transform.position + randomPos;
    }

    private void Start()
    {
        StartCoroutine(MonsterSpawn());
    }
}
