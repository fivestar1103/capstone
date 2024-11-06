using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    private NPCScript curNPC;
    public bool IsOpened { get; set; }
    
    public void OpenDialogue(NPCScript _npc)
    {
        curNPC = _npc;

        IsOpened = true;
        gameObject.SetActive(true);
        GameManager.SetControlMode(EControlMode.UI_CONTROL);
    }

    public void CloseDialogue()
    {
        curNPC.StopInteract();
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
