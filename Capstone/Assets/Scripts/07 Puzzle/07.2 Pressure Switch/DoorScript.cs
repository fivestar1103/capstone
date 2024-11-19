using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;

    private bool isOpen = false;
    private int needSwitchNumbers = 2;
    private List<PressureSwitchScript> pressureSwitches = new();

    public void AddPressureSwitch(PressureSwitchScript pressureSwitch)
    {
        if (!pressureSwitches.Contains(pressureSwitch))
        {
            pressureSwitches.Add(pressureSwitch);

            if (pressureSwitches.Count == needSwitchNumbers)
                OpenDoor();
        }
    }

    public void RemovePressureSwitch(PressureSwitchScript pressureSwitch)
    {
        if (pressureSwitches.Contains(pressureSwitch))
        {
            pressureSwitches.Remove(pressureSwitch);
        }
    }

    private void OpenDoor()
    {
        isOpen = true;

        Debug.Log("문이 열림");

        doorObject.SetActive(false);
    }
}
