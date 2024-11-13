using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    public GameObject cellsParent;
    public GameObject cellPrototypePrefab;
    public GameObject roomInfoPanelParent;
    public GameObject roomInfoPanelPrefab;
    
    
    private void Start()
    {
        var mapGenerator = new MapGenerator(100, 
            100, 
            0.35f, 
            3, 
            4, 
            3, 
            15);
        var mapDisplayer = new MapDisplayer(mapGenerator.Width,
            mapGenerator.Height,
            cellsParent,
            cellPrototypePrefab,
            roomInfoPanelParent,
            roomInfoPanelPrefab);

        var rawMap = mapGenerator.GenerateMap();
        
        Map roomMap;
        List<Room> rooms;
        (roomMap, rooms) = mapGenerator.GenerateRooms(rawMap);

        var wallMap = mapGenerator.GenerateWalls(roomMap, rooms);
        
        foreach (var room in rooms) 
            room.CalculateRoomInfo();
        
        mapDisplayer.LogMap(wallMap);
        mapDisplayer.LogRoomInfo(rooms);
    }

}
