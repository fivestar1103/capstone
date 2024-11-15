using System.Collections;
using System.Collections.Generic;
using PCG.Data_Structures;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    [Tooltip("Step by step generation of the map")] public bool stepByStep = false; 
    
    public GameObject cellsParent;
    public GameObject cellPrototypePrefab;
    public GameObject roomInfoPanelParent;
    public GameObject roomInfoPanelPrefab;
    public TextMeshProUGUI generationText;

    [Range(10, 200)] public int width = 100;
    [Range(10, 200)] public int height = 100;
    [Range(0f, 1f)] public float roomPercentage = 0.35f;
    [Range(1, 20)] public int generations = 3;
    [Range(0, 8)] public int birthLimit = 3;
    [Range(0, 8)] public int deathLimit = 4;
    [Range(0, 100)] public int roomThreshold = 25;
    
    private MapGenerator mapGenerator;
    private Map map;
    int currentGeneration = 0;

    private void Start()
    {
        if (stepByStep) return;
        
        InstantlyGenerateMap();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (stepByStep)
            {
                currentGeneration = 0;
                (mapGenerator, map) = InitiateMap();
                DisplayMap(null, map);
            }
            else
                InstantlyGenerateMap();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && currentGeneration < generations)
        {
            currentGeneration++;
            map = mapGenerator.CellularAutomata(map);
            if (currentGeneration == generations)
            {
                var (wallMap, roomsWithWalls) = PostProcessMap(mapGenerator, map);
                DisplayMap(roomsWithWalls, wallMap);
            }
            else 
                DisplayMap(null, map);
        }
    }

    private void InstantlyGenerateMap()
    {
        (mapGenerator, map) = InitiateMap();
        currentGeneration = generations;
        for (var i = 0; i < generations; i++)
            map = mapGenerator.CellularAutomata(map);
        var (wallMap, roomsWithWalls) = PostProcessMap(mapGenerator, map);
        DisplayMap(roomsWithWalls, wallMap);
    }

    private (MapGenerator, Map) InitiateMap()
    {
        var mapGenerator = new MapGenerator(width,
            height,
            roomPercentage,
            generations,
            birthLimit,
            deathLimit,
            roomThreshold);
        var rawMap = mapGenerator.GenerateMap();
        
        return (mapGenerator, rawMap);
    }
    
    private (Map, List<Room>) PostProcessMap(MapGenerator mapGenerator, Map rawMap)
    {
        Map roomMap;
        List<Room> rooms;
        (roomMap, rooms) = mapGenerator.GenerateRooms(rawMap);

        Map wallMap;
        List<Room> roomsWithWalls;
        (wallMap, roomsWithWalls) = mapGenerator.GenerateWalls(roomMap, rooms);
        
        return (wallMap, roomsWithWalls);
    }

    private void DisplayMap(List<Room> rooms, Map map)
    {
        var mapDisplayer = new MapDisplayer(mapGenerator.Width,
            mapGenerator.Height,
            cellsParent,
            cellPrototypePrefab,
            roomInfoPanelParent,
            roomInfoPanelPrefab,
            generationText);
        mapDisplayer.RemoveRoomInfoAndCells();

        mapDisplayer.LogMap(map, currentGeneration);
        if (rooms != null) mapDisplayer.LogRoomInfo(rooms);
    }

}
