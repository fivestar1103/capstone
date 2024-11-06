using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    private Slider hpBar;
    private TextMeshProUGUI HPInfo;

    private float MaxHP { get; set; }

    public void SetMaxHP(float _hp)
    {
        hpBar.maxValue = _hp;
        MaxHP = _hp;
        SetCurHP(_hp);
    }

    public void SetCurHP(float _hp)
    {
        hpBar.value = _hp;
        HPInfo.text = $"{_hp:F0} / {MaxHP:F0}";

        if (hpBar.value <= 0.3f) HPInfo.color = Color.red;
    }

    public void SetComps()
    {
        hpBar = GetComponentInChildren<Slider>();
        HPInfo = GetComponentInChildren<TextMeshProUGUI>();
    }
}
