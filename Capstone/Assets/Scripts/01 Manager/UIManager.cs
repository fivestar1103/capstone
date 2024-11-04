using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private DialogueUI dialogue;
    public bool IsDialogueOpened { get { return dialogue.IsOpened; } }
    public void OpenDialogue(NPCScript _npc) { dialogue.OpenDialogue(_npc); }
    public void CloseDialogue() { dialogue.CloseDialogue(); }

    [SerializeField]
    private SupporterUI support;
    public void ToggleSupporterUI(bool _toggle) { support.ToggleSupporterUI(_toggle); }

    [SerializeField]
    private HPbar hpbar;
    public void SetHPInfo(float _curHP) { hpbar.SetHPInfo(_curHP); }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
