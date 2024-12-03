using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePuzzleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject pressureSwitch;
    [SerializeField] private GameObject tileMap;

    private ConvertToMap convertToMap;
    private OrderDoorScript orderDoorScript;
    private Room pressurePuzzleRoom;

    private List<PuzzleTrigger> pressureSwitches;
    private int[,] pressurePuzzleMap;
    private int switchNumbers;              // switchNumbers is square number.
    private int switchNumberWidth;          // switchNumberWidth is sqrt(switchNumbers)

    private void Awake()
    {
        convertToMap = new ConvertToMap();
        orderDoorScript = new OrderDoorScript();
        pressureSwitches = new List<PuzzleTrigger>();
    }

    private void Start()
    {
        // switch numbers
        switchNumbers = orderDoorScript.needTriggerNumbers;
        switchNumberWidth = (int)(Mathf.Sqrt(switchNumbers));

        convertToMap.ConvertDataStructure(pressurePuzzleRoom, ref pressurePuzzleMap);
        convertToMap.DebugPrintMap(pressurePuzzleMap);

        (int x, int y) absoluteCenter = (pressurePuzzleRoom.CenterCell.X, pressurePuzzleRoom.CenterCell.Y);
        // (int x, int y) relativeCenter = (absoluteCenter.x - pressurePuzzleRoom.X,
        //                                  absoluteCenter.y - pressurePuzzleRoom.Y);

        SpawnSwitches(absoluteCenter);
    }

    // instantiate switches at right position
    // and add switches in list with randomly fixed order
    private void SpawnSwitches((int x, int y) spawnPoint)
    {
        (int x, int y) startPoint = (spawnPoint.x - switchNumberWidth / 2, spawnPoint.y - switchNumberWidth / 2);
        GameObject prefab;
        int[] randomArray = new int[switchNumbers];
        GenerateRandomArray(randomArray);

        for (int y = 0; y < switchNumberWidth; y++)
        {
            for (int x = 0; x < switchNumberWidth; x++)
            {
                prefab = Instantiate(pressureSwitch, tileMap.transform);
                prefab.transform.localPosition = new Vector3(startPoint.y + y * 4, 0, startPoint.x + x * 4);

                AddSwitches(prefab, randomArray[x + y * switchNumberWidth]);
            }
        }
    }

    private void AddSwitches(GameObject switchObject, int switchNumber)
    {
        PressureSwitch pressureSwitch = switchObject.GetComponent<PressureSwitch>();
        pressureSwitch.switchNum = switchNumber;
        pressureSwitch.cellPosition = switchObject.transform.localPosition;

        pressureSwitches.Add(pressureSwitch);
    }

    private void GenerateRandomArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i + 1;
        }

        int randomIndex = 0;
        int temp = 0;

        // Fisher-Yates Shuffle
        for (int i = 0; i < array.Length; i++)
        {
            randomIndex = Random.Range(i, array.Length);

            temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
