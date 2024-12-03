using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class OrderDoorScript : MonoBehaviour
{
    [SerializeField] private GameObject[] doorObjects;
    private List<PuzzleTrigger> puzzleTriggers = new();

    // needTriggerNumbers : square number
    public int needTriggerNumbers;

    public void AddTriggerObject(PuzzleTrigger triggerObject)
    {
        if (!puzzleTriggers.Contains(triggerObject))
        {
            puzzleTriggers.Add(triggerObject);

            

            if (CheckOrder())
            {
                if (puzzleTriggers.Count == needTriggerNumbers)
                    OpenDoor();
            }
            else
            {
                puzzleTriggers.Clear();
            }
        }
    }

    private bool CheckOrder()
    {
        bool rightOrderFlag = true;
        int index = 0;

        foreach (var puzzleTrigger in puzzleTriggers)
        {
            index++;
            if (puzzleTrigger.switchNum != index)
            {
                rightOrderFlag = false;
                break;
            }
        }

        return rightOrderFlag;
    }

    private void OpenDoor()
    {
        Debug.Log("문이 열림");

        foreach (var doorObject in doorObjects)
            doorObject.SetActive(false);
    }
}
