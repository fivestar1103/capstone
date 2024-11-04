using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour, IInteractable
{
    public InteractScript InteractManager { get; private set; }

    public bool CanInteract { get { return true; } }

    public string InfoTxt { get { return "대화"; } }

    public void SetInteractScript(InteractScript _script)
    {
        InteractManager = _script;
    }

    public virtual void StartInteract()
    {
        Debug.Log(this.name + " 상호작용 시작");
        PlayManager.OpenDialogue(this);
    }

    public virtual void StopInteract()
    {
        Debug.Log(this.name + " 상호작용 종료");
        PlayManager.StopPlayerInteract();
        GameManager.SetControlMode(EControlMode.FIRST_PERSON);
    }

    private void Start()
    {
        
    }
}
