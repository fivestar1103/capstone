using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPbar : MonoBehaviour
{
    private TextMeshProUGUI HPInfo;

    public void SetHPInfo(float _curHP)
    {
        HPInfo.text = $"{_curHP} / {PlayManager.MaxHP}";
    }

    private void Start()
    {
        HPInfo = GetComponentInChildren<TextMeshProUGUI>();
    }
}
