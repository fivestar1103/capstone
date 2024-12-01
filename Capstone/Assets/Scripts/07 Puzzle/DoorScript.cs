using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private GameObject doorObject;
    [SerializeField] private int needTriggerNumbers;
    private List<IPuzzleTrigger> pressureTriggers = new();

    public void AddTriggerObject(IPuzzleTrigger triggerObject)
    {
        if (!pressureTriggers.Contains(triggerObject))
        {
            pressureTriggers.Add(triggerObject);

            if (pressureTriggers.Count == needTriggerNumbers)
                OpenDoor();
        }
    }

    public void RemoveTriggerObject(IPuzzleTrigger triggerObject)
    {
        if (pressureTriggers.Contains(triggerObject))
        {
            pressureTriggers.Remove(triggerObject);
        }
    }

    private void OpenDoor()
    {
        Debug.Log("문이 열림");

        doorObject.SetActive(false);
    }
}
