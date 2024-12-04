using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNum;

    public void SetBattleInfo()
    {
        this.gameObject.SetActive(true);
        monsterNum.text = $"{PlayManager.MonsterNum} / {PlayManager.TotalMonsterNum}";
    }

    private void Start()
    {
        monsterNum.text = $"{PlayManager.MonsterNum} / {PlayManager.TotalMonsterNum}";
    }
}
