using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : PuzzleTrigger
{
    public OrderDoor Door;
    public int switchNumber { get { return base.switchNum; } }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("¹â´Â Áß!");
        Door.AddTriggerObject(this);
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    Debug.Log("¶¼´Â Áß!");
    //    Door.RemoveTriggerObject(this);
    //}
}
