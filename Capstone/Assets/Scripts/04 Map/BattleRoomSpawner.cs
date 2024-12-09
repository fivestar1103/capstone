using PCG.Data_Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BattleRoomSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monster;

    [SerializeField]
    private MonsterSpawnPoint spawnPoint;   // ¸ó½ºÅÍ ½ºÆ÷³Ê ÇÁ¸®ÆÕ
    [SerializeField]
    private int battleRoomCount = 10;       // ÀüÅõ¹æ °¹¼ö
    private int pointCount;                 // ¹æ ÇÏ³ª¿¡ ¼³Ä¡µÉ ¸ó½ºÅÍ ½ºÆ÷³ÊÀÇ °¹¼ö
    private int maxRoomNumber;              // ¹æ ¹øÈ£ Áß °¡Àå Å« ¼ö

    private HashSet<int> battleRoomNumber = new HashSet<int>();           // ÀüÅõ¹æÀ¸·Î »ç¿ëµÉ ¹æ ¹øÈ£µé
    private List<Room> roomInfo = new List<Room>();                       // ÀüÅõ¹æ list

    // ÇÃ·¹ÀÌ¾î°¡ ¹æ¿¡ µé¾î°¥ ¶§ È°¼ºÈ­µÇ´Â ¿ÀºêÁ§Æ®µé
    [SerializeField]
    private BattleTriggerScript battleTrigger;
    private GameObject roomObject;                                        // navigation ´ã´ç ¿ÀºêÁ§Æ®
    private NavMeshSurface navMeshSurface;                                // navigation ´ã´ç ¿ÀºêÁ§Æ®2

    private int monsterSpawnCount;
    public int MonsterSpawnCount
    {
        get { return monsterSpawnCount; } 
        set { monsterSpawnCount = value; } 
    }

    private List<Vector3> SelectRandomPosition(HashSet<Vector3> set, int count)
    {
        // HashSetÀ» List·Î º¯È¯
        List<Vector3> list = new List<Vector3>(set);

        // Shuffle (¸®½ºÆ®¸¦ ·£´ýÇÏ°Ô ¼¯À½)
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = list[i]; 
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        // ¸®½ºÆ®¿¡¼­ »óÀ§ 'count'°³ÀÇ °ª ¹ÝÈ¯
        return list.GetRange(0, Mathf.Min(count, list.Count));
    }

    public void SpawnBattleRoom(Room room)
    {
        HashSet<Vector3> spawnerPosInfo = new HashSet<Vector3>();

        #region Setting BattleObject
        foreach (var cell in room.RoomCells)
        {
            if (!cell.IsCenter)
            {
                Vector3 realSpawnerPos = new Vector3(cell.X * 4, 0.01f, cell.Y * -4);
                spawnerPosInfo.Add(realSpawnerPos);
            }
            else // Áß¾Ó¿¡´Â NavMeshSurface ´ã´ç ¿ÀºêÁ§Æ® ¹èÄ¡
            {
                roomObject = new GameObject("Room" + room.RoomNumber);
                roomObject.transform.position = new Vector3(cell.X * 4, 1.0f, cell.Y * -4);

                navMeshSurface = roomObject.AddComponent<NavMeshSurface>();
                navMeshSurface.collectObjects = CollectObjects.Children; // ÀÚ½Ä ¿ÀºêÁ§Æ®¸¸ NavMesh·Î Æ÷ÇÔ
            }
        }

        monsterSpawnCount = Random.Range(room.RoomCells.Count / 15, room.RoomCells.Count / 10);
        List<Vector3> randomPositions = SelectRandomPosition(spawnerPosInfo, monsterSpawnCount);

        foreach (var position in randomPositions)
        {
            GameObject spawner = Instantiate(spawnPoint.gameObject, position, Quaternion.identity);
            spawner.transform.SetParent(roomObject.transform);
            spawner.SetActive(false);
            room.MonsterSpawners.Add(spawner);
        }

        foreach (var corridor in room.CorridorCells)
        {
            Vector3 triggerPos = new Vector3(corridor.X * 4, 1f, corridor.Y * -4);

            BattleTriggerScript triggerInstance = Instantiate(battleTrigger, triggerPos, Quaternion.identity);
            triggerInstance.transform.SetParent(roomObject.transform); 
            triggerInstance.GetComponent<Collider>().isTrigger = true;
            
            triggerInstance.SetBattleRoom(this, room.RoomNumber);
        }
        #endregion
    }

    public void ActivateBattleObject(int _roomNumber)
    {
        foreach(var room in PlayManager.RoomWithWalls)
        {
            if(_roomNumber == room.RoomNumber && room.MonsterSpawners.Count > 0)
            {
                PlayManager.MonsterSpawnerCount = room.MonsterSpawners.Count;

                foreach (var spawner in room.MonsterSpawners)
                {
                    spawner.gameObject.SetActive(true);
                }

                #region Setting BattleRoom Navigation
                foreach (var tile in room.RoomCellObjectsDictionary)
                {
                    GameObject tileObject = tile.Value;
                    tileObject.transform.parent = roomObject.transform;

                    NavMeshLink link = tileObject.AddComponent<NavMeshLink>();

                    link.startPoint = new Vector3(-2.0f, 0, 0);
                    link.endPoint = new Vector3(2.0f, 0, 0);
                    link.width = 4.0f;
                    link.bidirectional = true;
                }
                navMeshSurface.BuildNavMesh();
                #endregion
            }
        }
    }

    public void SetRoomData(List<Room> roomsWithWalls)
    {
        foreach (var room in roomsWithWalls)
        {
            roomInfo.Add(room);
        }

         // ·£´ýÇÑ battleRoomCount°³ÀÇ ¹æÀ» ÀüÅõ ¹æÀ¸·Î ¼³Á¤
         maxRoomNumber = roomInfo.Max(room => room.RoomNumber);
         while (battleRoomNumber.Count < battleRoomCount)
         {
             int randomRoomValue = Random.Range(0, maxRoomNumber + 1);
             if (randomRoomValue == 0) continue;
             battleRoomNumber.Add(randomRoomValue);
         }

         foreach (var room in roomInfo)
         {
             if (battleRoomNumber.Contains(room.RoomNumber))  // ÀüÅõ ¹æ setting
             {
                 room.Type = RoomType.Battle;
                 SpawnBattleRoom(room);
             }
         }

         // #region Visualization
         // foreach (var room in roomInfo)
         // {
         //     // Create a 2D array representing the room including walls
         //     var display = new char[room.Height + 2, room.Width + 2];
         //
         //     // Initialize with empty spaces
         //     for (int y = 0; y < room.Height + 2; y++)
         //     {
         //         for (int x = 0; x < room.Width + 2; x++)
         //         {
         //             display[y, x] = ' ';
         //         }
         //     }
         //
         //     // Add room cells
         //     foreach (var cell in room.RoomCells)
         //     {
         //         var relativeX = cell.X - room.X;
         //         var relativeY = cell.Y - room.Y;
         //         display[relativeY, relativeX] = cell.IsCenter ? 'M' : 'R';
         //     }
         //
         //     // Add wall cells
         //     foreach (var cell in room.WallCells)
         //     {
         //         var relativeX = cell.X - room.X;
         //         var relativeY = cell.Y - room.Y;
         //         display[relativeY, relativeX] = 'W';
         //     }
         //
         //     // Add corridor cells
         //     foreach (var cell in room.CorridorCells)
         //     {
         //         var relativeX = cell.X - room.X;
         //         var relativeY = cell.Y - room.Y;
         //         display[relativeY, relativeX] = 'C';
         //     }
         //
         //     var roomLayout = "\n";
         //
         //     // Add top border
         //     roomLayout += "+";
         //     for (int x = 0; x < room.Width + 2; x++)
         //         roomLayout += "-";
         //     roomLayout += "+\n";
         //
         //     // Add room content with side borders
         //     for (int y = 0; y < room.Height + 2; y++)
         //     {
         //         roomLayout += "|";
         //         for (int x = 0; x < room.Width + 2; x++)
         //         {
         //             roomLayout += display[y, x];
         //         }
         //         roomLayout += "|\n";
         //     }
         //
         //     // Add bottom border
         //     roomLayout += "+";
         //     for (int x = 0; x < room.Width + 2; x++)
         //         roomLayout += "-";
         //     roomLayout += "+\n";
         //
         //
         //     // Print room information
         //     // Debug.Log($"Room #{room.RoomNumber} - Type: {room.Type}\n" +
         //     //           $"Position: ({room.X}, {room.Y}), Size: {room.Width}x{room.Height}\n" +
         //     //           $"Center Cell: ({room.CenterCell.X}, {room.CenterCell.Y})\n" +
         //     //           "Room Layout (W=Wall, R=Room, M=Center, C=Corridor):\n" +
         //     //           $"{roomLayout}");
         // }
         // #endregion

     }

    private void Start()
    {
        
    }
}
