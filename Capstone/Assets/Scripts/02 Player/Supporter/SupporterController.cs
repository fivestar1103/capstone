using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupporterController : NPCScript
{
    [SerializeField]
    private PlayerController player;

    private void Start()
    {
        
    }

    private void Update()
    {
        MatchPlayerPos();
    }

    private void MatchPlayerPos()
    {
        Vector3 newPos = new Vector3((PlayManager.PlayerPos.x + 1), transform.position.y, (PlayManager.PlayerPos.z - 1));    
        transform.position = newPos;   
    }
}
