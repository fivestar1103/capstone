using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    private Slider hpBar;
    private TextMeshProUGUI HPInfo;

    public void SetHPInfo(float _curHP)
    {
        hpBar.value = _curHP / PlayManager.MaxHP;
        HPInfo.text = $"{_curHP} / {PlayManager.MaxHP}";

        if (hpBar.value <= 0.3f) HPInfo.color = Color.red;
    }

    private void Start()
    {
        hpBar = GetComponent<Slider>();
        HPInfo = GetComponentInChildren<TextMeshProUGUI>();
    }
}
