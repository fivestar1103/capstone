using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTriggerScript : MonoBehaviour
{
    private BattleRoomSpawner battleRoomSpawner;
    private Room curRoom;

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag(ValueDefinition.PLAYER_TAG) && !curRoom.IsBattleStarted)
        {
            PlayManager.MonsterNum = 0;
            battleRoomSpawner.ActivateBattleObject(curRoom);
            StartBattle();
        }
    }

    public void SetBattleRoom(BattleRoomSpawner _battleRoom, Room _room)
    {
        battleRoomSpawner = _battleRoom;
        curRoom = _room;
    }



    public void StartBattle()
    {
        curRoom.IsBattleStarted = true;
        PlayManager.ShowBattleUI();
        PlayManager.SetBattleInfo(curRoom);
        PlayManager.StartTimer(curRoom);
    }
}
