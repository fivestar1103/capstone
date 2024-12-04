using PCG.Data_Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BattleRoomSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private MonsterSpawnPoint spawnPoint;   // 몬스터 스포너 프리팹
    [SerializeField]
    private int battleRoomCount = 10;       // 전투방 갯수
    private int pointCount;                 // 방 하나에 설치될 몬스터 스포너의 갯수
    private int maxRoomNumber;              // 방 번호 중 가장 큰 수

    private HashSet<int> battleRoomNumber = new HashSet<int>();           // 전투방으로 사용될 방 번호들
    private List<Room> roomInfo = new List<Room>();                       // 전투방 list
    private GameObject roomObject;                                        // navigation 담당 오브젝트
    private NavMeshSurface navMeshSurface;                                // navigation 담당 오브젝트2

    // 플레이어가 ? 호출 -> 몬스터 생성

    private List<Vector3> SelectRandomPosition(HashSet<Vector3> set, int count)
    {
        // HashSet을 List로 변환
        List<Vector3> list = new List<Vector3>(set);

        // Shuffle (리스트를 랜덤하게 섞음)
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = list[i]; 
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        // 리스트에서 상위 'count'개의 값 반환
        return list.GetRange(0, Mathf.Min(count, list.Count));
    }

    public void SpawnBattleRoom(Room room)
    {
        HashSet<Vector3> spawnerPosInfo = new HashSet<Vector3>();

        #region Setting MonsterSpawner
        foreach (var cell in room.RoomCells)
        {
            if(!cell.IsCenter)
            {
                Vector3 RealSpawnerPos = new Vector3(cell.X * 4, 0.01f, cell.Y * -4);
                spawnerPosInfo.Add(RealSpawnerPos);
            }
            else
            {
                roomObject = new GameObject("Room" + room.RoomNumber);
                roomObject.transform.position = new Vector3(cell.X * 4, 1.0f, cell.Y * -4);

                navMeshSurface = roomObject.AddComponent<NavMeshSurface>();
                navMeshSurface.collectObjects = CollectObjects.Children; // 자식 오브젝트만 NavMesh로 포함
            }
        }

        int count = 3; // 임의로 3개 값 선택(temp)
        List<Vector3> randomPositions = SelectRandomPosition(spawnerPosInfo, count);

        foreach (var position in randomPositions)
        {
            GameObject spawner = Instantiate(spawnPoint.gameObject, position, Quaternion.identity);
            spawner.SetActive(true);
        }
        #endregion

        #region Setting BattleRoom
        foreach (var tile in room.RoomCellObjectsDictionary)
        {
            GameObject tileObject = tile.Value;
            tileObject.transform.parent = roomObject.transform;

            // 3. 타일 간 연결을 위한 NavMeshLink 추가
            NavMeshLink link = tileObject.AddComponent<NavMeshLink>();

            // 경계 설정 (가로 방향 예제)
            link.startPoint = new Vector3(-2.0f, 0, 0); // 타일 왼쪽 경계
            link.endPoint = new Vector3(2.0f, 0, 0);   // 타일 오른쪽 경계
            link.width = 4.0f; // 링크 폭
            link.bidirectional = true; // 양방향 이동 허용
        }
        navMeshSurface.BuildNavMesh();
        #endregion
    }

    public void SetRoomData(List<Room> roomsWithWalls)
    {
        foreach (var room in roomsWithWalls)
        {
            roomInfo.Add(room);
        }

        // 랜덤한 battleRoomCount개의 방을 전투 방으로 설정
        maxRoomNumber = roomInfo.Max(room => room.RoomNumber);
        while (battleRoomNumber.Count < battleRoomCount)
        {
            int randomRoomValue = Random.Range(0, maxRoomNumber + 1);
            if (randomRoomValue == 0) continue;
            battleRoomNumber.Add(randomRoomValue);
        }

        foreach (var room in roomInfo)
        {
            if (battleRoomNumber.Contains(room.RoomNumber))  // 전투 방 setting
            {
                room.Type = RoomType.Battle;
                SpawnBattleRoom(room);
            }
        }

        #region Visualization
        foreach (var room in roomInfo)
        {
            // Create a 2D array representing the room including walls
            var display = new char[room.Height + 2, room.Width + 2];

            // Initialize with empty spaces
            for (int y = 0; y < room.Height + 2; y++)
            {
                for (int x = 0; x < room.Width + 2; x++)
                {
                    display[y, x] = ' ';
                }
            }

            // Add room cells
            foreach (var cell in room.RoomCells)
            {
                var relativeX = cell.X - room.X;
                var relativeY = cell.Y - room.Y;
                display[relativeY, relativeX] = cell.IsCenter ? 'M' : 'R';
            }

            // Add wall cells
            foreach (var cell in room.WallCells)
            {
                var relativeX = cell.X - room.X;
                var relativeY = cell.Y - room.Y;
                display[relativeY, relativeX] = 'W';
            }

            // Add corridor cells
            foreach (var cell in room.CorridorCells)
            {
                var relativeX = cell.X - room.X;
                var relativeY = cell.Y - room.Y;
                display[relativeY, relativeX] = 'C';
            }

            var roomLayout = "\n";

            // Add top border
            roomLayout += "+";
            for (int x = 0; x < room.Width + 2; x++)
                roomLayout += "-";
            roomLayout += "+\n";

            // Add room content with side borders
            for (int y = 0; y < room.Height + 2; y++)
            {
                roomLayout += "|";
                for (int x = 0; x < room.Width + 2; x++)
                {
                    roomLayout += display[y, x];
                }
                roomLayout += "|\n";
            }

            // Add bottom border
            roomLayout += "+";
            for (int x = 0; x < room.Width + 2; x++)
                roomLayout += "-";
            roomLayout += "+\n";


            // Print room information
            Debug.Log($"Room #{room.RoomNumber} - Type: {room.Type}\n" +
                      $"Position: ({room.X}, {room.Y}), Size: {room.Width}x{room.Height}\n" +
                      $"Center Cell: ({room.CenterCell.X}, {room.CenterCell.Y})\n" +
                      "Room Layout (W=Wall, R=Room, M=Center, C=Corridor):\n" +
                      $"{roomLayout}");
        }
        #endregion

    }

    private void Start()
    {

    }
}
