using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchScript : MonoBehaviour
{
    [SerializeField] private PSDoorScript Door;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("¹â´Â Áß!");
        Door.AddPressureSwitch(this);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("¶¼´Â Áß!");
        Door.RemovePressureSwitch(this);
    }
}
