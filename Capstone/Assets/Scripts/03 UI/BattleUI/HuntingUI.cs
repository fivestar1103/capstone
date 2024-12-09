using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HuntingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNum;
    private int totalMonsterNum;
    private Room curRoom;

    public void SetBattleInfo() // 몬스터가 죽었을 때의 갱신
    {
        monsterNum.text = $"{PlayManager.MonsterNum} / {totalMonsterNum}";

        // 전투 끝
        if (PlayManager.MonsterNum == totalMonsterNum)
        {
            PlayManager.FinishBattle(curRoom);
            this.gameObject.SetActive(false);
        }
    }

    public void SetBattleInfo(Room _room) // 방에 처음 입장했을 때의 갱신
    {
        curRoom = _room;
        this.gameObject.SetActive(true);
        monsterNum.text = $"{PlayManager.MonsterNum} / {totalMonsterNum}";
    }

    private void OnEnable()
    {
        totalMonsterNum = PlayManager.MonsterSpawnerCount * 5;
        // totalMonsterNum = 2;
    }
}
