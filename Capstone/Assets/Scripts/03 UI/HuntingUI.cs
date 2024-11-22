using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterNum;

    public void SetMonsterNum()
    {
        monsterNum.text = $"{PlayManager.MonsterNum} / {PlayManager.TotalMonsterNum}";
    }

    private void Start()
    {
        monsterNum.text = $"{PlayManager.MonsterNum} / {PlayManager.TotalMonsterNum}";
    }
}
