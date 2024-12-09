using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNum;
    private int totalMonsterNum;

    public void SetBattleInfo()
    {
        // totalMonsterNum = PlayManager.MonsterSpawnerCount * 5;

        this.gameObject.SetActive(true);
        monsterNum.text = $"{PlayManager.MonsterNum} / {totalMonsterNum}";

        if(PlayManager.MonsterNum == totalMonsterNum)
        {
            this.gameObject.SetActive(false);
            // PlayManager.FinishBattle();
        }
    }

    private void OnEnable()
    {
        totalMonsterNum = PlayManager.MonsterSpawnerCount * 5;
    }
}
