using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTriggerScript : MonoBehaviour
{
    private BattleRoomSpawner battleRoomSpawner;
    private int roomNumber;
    public bool IsBattleStarted { get; private set; }
    public bool IsBattleFinished { get; set; } 

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag(ValueDefinition.PLAYER_TAG) && !IsBattleStarted)
        {
            battleRoomSpawner.ActivateBattleObject(roomNumber);
            StartBattle();
        }
    }

    public void SetBattleRoom(BattleRoomSpawner _battleRoom, int _roomNumber)
    {
        battleRoomSpawner = _battleRoom;
        roomNumber = _roomNumber;
    }

    public void StartBattle()
    {
        PlayManager.SetBattleInfo();
        PlayManager.ShowBattleUI();
        PlayManager.StartTimer();
        IsBattleStarted = true;
    }

    public void FinishBattle()
    {

    }

}
