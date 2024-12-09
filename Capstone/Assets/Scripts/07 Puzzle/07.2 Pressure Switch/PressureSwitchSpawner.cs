using PCG.Data_Structures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pressureSwitch;

    private OrderDoor orderDoor;

    private List<PuzzleTrigger> pressureSwitches;
    private int switchNumbers;              // switchNumbers is square number.
    private int switchNumberWidth;          // switchNumberWidth is sqrt(switchNumbers)

    private void Awake()
    {
        orderDoor = new OrderDoor();
        pressureSwitches = new List<PuzzleTrigger>();

        PressurePuzzleManager.ASpawnSwitches += SpawnSwitches;

        // switch numbers
        // switchNumbers = orderDoor.needTriggerNumbers;
        switchNumbers = 9;
        switchNumberWidth = (int)(Mathf.Sqrt(switchNumbers));
    }

    // instantiate switches at right position
    // and add switches in list with randomly fixed order
    private void SpawnSwitches(Room room, GameObject tileMap, GameObject puzzleObject)
    {
        (int x, int y) spawnPoint = (room.CenterCell.X - switchNumberWidth / 2, room.CenterCell.Y - switchNumberWidth / 2);
        GameObject switchPrefab = null;
        int[] randomArray = new int[switchNumbers];
        GenerateRandomArray(randomArray);

        Debug.Log($"puzzle spawn point : {spawnPoint}");

        for (int y = 0; y < switchNumberWidth; y++)
        {
            for (int x = 0; x < switchNumberWidth; x++)
            {
                var switchPoint = (x: spawnPoint.x + x, y: spawnPoint.y + y);
                Debug.Log($"puzzle switch point : {switchPoint}");

                var roomCellObject = tileMap.transform.Find($"Room ({switchPoint.x}, {switchPoint.y})").gameObject;
                if (!roomCellObject)
                    continue;

                Debug.Log($"puzzle roomCell object : {roomCellObject}");
                Destroy(roomCellObject);

                switchPrefab = Instantiate(pressureSwitch, puzzleObject.transform);
                switchPrefab.transform.localPosition = new Vector3(switchPoint.x * 4, 0, -switchPoint.y * 4);

                Debug.Log($"puzzle switch prefab : {switchPrefab}");
                // room.RoomCellObjectsDictionary[roomCell] = switchPrefab;

                AddSwitches(switchPrefab, randomArray[x + y * switchNumberWidth]);
            }
        }
    }

    private void AddSwitches(GameObject switchObject, int switchNumber)
    {
        PressureSwitch pressureSwitch = switchObject.GetComponentInChildren<PressureSwitch>();
        // pressureSwitch.Door = 
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
