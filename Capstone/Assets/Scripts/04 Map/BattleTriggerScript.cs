using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTriggerScript : MonoBehaviour
{
    private BattleRoomSpawner battleRoomSpawner;
    private int roomNumber;
    public bool IsBattleEnded { get; private set; }

    private void OnTriggerEnter(Collider _other)
    {
        if(_other.CompareTag(ValueDefinition.PLAYER_TAG) && !IsBattleEnded)
        {
            battleRoomSpawner.ActivateBattleObject(roomNumber);
            IsBattleEnded = true;
        }
    }

    public void SetBattleRoom(BattleRoomSpawner _battleRoom, int _roomNumber)
    {
        battleRoomSpawner = _battleRoom;
        roomNumber = _roomNumber;
    }
}
