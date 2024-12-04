using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTriggerScript : MonoBehaviour
{
    private BattleRoomSpawner battleRoomSpawner;
    private int roomNumber;
    public bool IsBattleStarted { get; private set; }

    //[SerializeField]
    //private GameObject battleStartUI;
    //[SerializeField]
    //private GameObject battleTimer;

    //private FadeinFadeout fade;
    //private TimerScript timer;

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
        //StartCoroutine(UIFade());
        //battleTimer.SetActive(true);
        IsBattleStarted = true;
    }

    //IEnumerator UIFade()
    //{
    //    yield return StartCoroutine(fade.FadeIn());
    //    yield return new WaitForSeconds(2.5f);
    //    yield return StartCoroutine(fade.FadeOut());
    //}

    //private void Start()
    //{
    //    fade = battleStartUI.GetComponent<FadeinFadeout>();
    //    timer = battleTimer.GetComponent<TimerScript>();
    //}

}
