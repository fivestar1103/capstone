using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public bool IsOpened { get; set; }

    public void OpenDialogue()
    {
        IsOpened = true;
        gameObject.SetActive(true);
    }

    public void CloseDialogue()
    {
        IsOpened = false;
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
