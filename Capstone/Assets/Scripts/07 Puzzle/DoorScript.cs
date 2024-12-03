using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private GameObject[] doorObjects;
    [SerializeField] private int needTriggerNumbers;
    private List<PuzzleTrigger> puzzleTriggers = new();

    public void AddTriggerObject(PuzzleTrigger triggerObject)
    {
        if (!puzzleTriggers.Contains(triggerObject))
        {
            puzzleTriggers.Add(triggerObject);

            if (puzzleTriggers.Count == needTriggerNumbers)
                OpenDoor();
        }
    }

    public void RemoveTriggerObject(PuzzleTrigger triggerObject)
    {
        if (puzzleTriggers.Contains(triggerObject))
        {
            puzzleTriggers.Remove(triggerObject);
        }
    }

    private void OpenDoor()
    {
        Debug.Log("문이 열림");

        foreach (var doorObject in doorObjects)
            doorObject.SetActive(false);
    }
}
