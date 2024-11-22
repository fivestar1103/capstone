using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas playerUICanvas;

    [SerializeField]
    private DialogueUI dialogue;
    public bool IsDialogueOpened { get { return dialogue.IsOpened; } }
    public void OpenDialogue(NPCScript _npc) { dialogue.OpenDialogue(_npc); }
    public void CloseDialogue() { dialogue.CloseDialogue(); }

    [SerializeField]
    private SupporterUI support;
    public void ToggleSupporterUI(bool _toggle) { support.ToggleSupporterUI(_toggle); }

    [SerializeField]
    private HuntingUI huntingInfo;
    public void SetMonsterNum() { huntingInfo.SetMonsterNum(); }

    private HPbar hpBar;
    public void SetMaxHP(float _hp) { hpBar.SetMaxHP(_hp); }
    public void SetCurHP(float _hp) { hpBar.SetCurHP(_hp); }

    public void SetManager()
    {
        hpBar = playerUICanvas.GetComponentInChildren<HPbar>();
        hpBar.SetComps();
    }
}
