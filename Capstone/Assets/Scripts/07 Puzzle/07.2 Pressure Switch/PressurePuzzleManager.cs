using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PCG.Data_Structures;
using UnityEngine.Tilemaps;

public class PressurePuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject tileMap;
    [SerializeField] private GameObject puzzlePrefab;
    [SerializeField] private OrderDoor Door;

    public static Action<Room, GameObject, GameObject> ASpawnSwitches;
    
    // 해결해야 할 점
    // 1. 퍼즐 스폰 포인트 정하기 -> 랜덤으로, 벽으로 막히는 일 없이 (퍼즐은 3x3으로 뭉쳐서 생성됨)
    // 2. door 스포너 만들어서 room에 진입했을 때 door가 생성되어서 출입구가 막히고, 퍼즐을 해결하면 출입구가 열려야 함
    // 3. door들이 생성되면 OrderDoor 스크립트에서 오브젝트들을 관리해야 함.
    // 4. PressureSwitch가 OrderDoor 스크립트에 접근 가능해야 함.
    // -> 아니면 OrderDoor 스크립트를 다른 상위 오브젝트에서 접근하고, PressureSwitch는 상위 오브젝트로 정보를 건네는 것으로 해결해야 함

    public void SpawnPuzzle(Room room)
    {
        // spawn puzzles in rooms at centerCell
        if (room.CenterCell == null)
            return;

        (int x, int y) centerPos = (room.CenterCell.X, room.CenterCell.Y);
        GameObject centerObject = GameObject.Instantiate(puzzlePrefab, tileMap.transform);
        centerObject.transform.localPosition = new Vector3(centerPos.x * 4, 0, -centerPos.y * 4);

        Debug.Log($"puzzle center position : {centerPos}");

        // spawn switches in puzzles
        ASpawnSwitches(room, tileMap, centerObject);
    }

    private (int, int) DecideSpawnPoint(Room room)
    {
        (int, int) spawnPoint = (0, 0);



        return spawnPoint;
    }
}
