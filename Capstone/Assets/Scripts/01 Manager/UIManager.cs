using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private DialogueUI dialogue;
    public void OpenDialogue() { dialogue.OpenDialogue(); }
    public void CloseDialogue() { dialogue.CloseDialogue(); }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
