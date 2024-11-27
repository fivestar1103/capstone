using System.Collections;
using System.Collections.Generic;
using PCG.Data_Structures;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Main : MonoBehaviour
{
    [Tooltip("Step by step generation of the map")] public bool stepByStep; 
    [Tooltip("Step by step generation of the Delaunay triangulation")] public bool pointByPoint;
    [Tooltip("Click to generate minimum spanning tree")] public bool vertexByVertex;
    
    public GameObject cellsParent;
    public GameObject groundPrototypePrefab;
    public GameObject midpointPrototypePrefab;
    public GameObject roomInfoPanelParent;
    public GameObject roomInfoPanelPrefab;
    public GameObject wallPrototypePrefab;
    public GameObject corridorPrototypePrefab;
    public GameObject edgesParent;
    
    public TextMeshProUGUI generationText;

    [Range(10, 200)] public int width = 100;
    [Range(10, 200)] public int height = 100;
    [Range(0f, 1f)] public float roomPercentage = 0.35f;
    [Range(1, 20)] public int generations = 3;
    [Range(0, 8)] public int birthLimit = 3;
    [Range(0, 8)] public int deathLimit = 4;
    [Range(0, 100)] public int roomThreshold = 25;
    
    private MapGenerator mapGenerator;
    private MapDisplayer mapDisplayer;
    private Map map;
    int currentGeneration = -1;
    
    DelaunayTriangulation delaunayTriangulation;
    HashSet<Edge> delaunayEdges;

    private List<Edge> minimumSpanningTreeEdges;
    private bool isMinimumSpanningTreeDone;

    private readonly PathFinder pathFinder = new PathFinder();
    
    /*
     * 100, 100, 0.15, 6, 3, 1, 30
     * 100, 100, 0.35, 5, 4, 3, 20
     */
    
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
                delaunayTriangulation = new DelaunayTriangulation(new List<Room>());
                delaunayTriangulation.DeleteAllEdges(edgesParent.transform);
                currentGeneration = 0;
                (mapGenerator, map) = InitiateMap();
                DisplayMap(null);
            }
            else
                InstantlyGenerateMap();
        }
        else if (Input.GetKeyDown(KeyCode.C) && 0 <= currentGeneration && currentGeneration < generations)
        {
            currentGeneration++;
            Debug.Log($"Generation {currentGeneration}");
            map = mapGenerator.CellularAutomata(map);
            if (currentGeneration == generations)
            {
                isMinimumSpanningTreeDone = false;
                var (wallMap, roomsWithWalls) = PostProcessMap();
                map = wallMap;
                DisplayMap(roomsWithWalls);
                
                delaunayTriangulation.DisplayDelaunayEdges(delaunayEdges, edgesParent.transform);
            }
            else 
                DisplayMap(null);
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentGeneration >= generations)
        {
            GetDelaunayEdges();
            delaunayTriangulation.DisplayDelaunayEdges(delaunayEdges, edgesParent.transform);
            isMinimumSpanningTreeDone = false;
            if (delaunayTriangulation.isDone && !vertexByVertex)
            {
                var minimumSpanningTree = new MinimumSpanningTree();
                minimumSpanningTreeEdges = minimumSpanningTree.GenerateMinimumSpanningTree(
                    delaunayEdges, delaunayTriangulation.MidPoints);
                delaunayTriangulation.DeleteNonMinimumSpanningTreeEdges(minimumSpanningTreeEdges,
                    edgesParent.transform);
                isMinimumSpanningTreeDone = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            // Debug.Log($"currentGeneration: {currentGeneration}, generations: {generations}, delaunayTriangulation.isDone: {delaunayTriangulation.isDone}, IsMinimumSpanningTreeDone: {isMinimumSpanningTreeDone}");
            if (currentGeneration >= generations &&
                delaunayTriangulation.isDone &&
                !isMinimumSpanningTreeDone)
            {
                var minimumSpanningTree = new MinimumSpanningTree();
                minimumSpanningTreeEdges = minimumSpanningTree.GenerateMinimumSpanningTree(
                    delaunayEdges, delaunayTriangulation.MidPoints);
                delaunayTriangulation.DeleteNonMinimumSpanningTreeEdges(minimumSpanningTreeEdges,
                    edgesParent.transform);
                isMinimumSpanningTreeDone = true;
            }
        }
    }

    private IEnumerator InstantlyGenerateMapCoroutine()
    {
        isMinimumSpanningTreeDone = false;
        (mapGenerator, map) = InitiateMap();
        currentGeneration = generations;
        for (var i = 0; i < generations; i++)
            map = mapGenerator.CellularAutomata(map);
        var (wallMap, roomsWithWalls) = PostProcessMap();
        map = wallMap;
        DisplayMap(roomsWithWalls);
    
        delaunayTriangulation.DisplayDelaunayEdges(delaunayEdges, edgesParent.transform);

        if (vertexByVertex) yield break;
    
        var minimumSpanningTree = new MinimumSpanningTree();
        minimumSpanningTreeEdges = minimumSpanningTree.GenerateMinimumSpanningTree(delaunayEdges,
            delaunayTriangulation.MidPoints);
        delaunayTriangulation.DeleteNonMinimumSpanningTreeEdges(minimumSpanningTreeEdges, edgesParent.transform);
    
        // Wait for one frame to ensure walls are properly initialized
        yield return null;

        List<Path> paths;
        (paths, roomsWithWalls) = pathFinder.CreatePath(minimumSpanningTreeEdges, roomsWithWalls);
        
        map = pathFinder.FindPath(map, paths);
        mapDisplayer.DisplayCorridors(map);
    }

    private void InstantlyGenerateMap()
    {
        StartCoroutine(InstantlyGenerateMapCoroutine());
    }

    private (MapGenerator, Map) InitiateMap()
    {
        mapGenerator = new MapGenerator(width,
            height,
            roomPercentage,
            generations,
            birthLimit,
            deathLimit,
            roomThreshold);
        var rawMap = mapGenerator.GenerateMap();
        
        return (mapGenerator, rawMap);
    }
    
    private (Map, List<Room>) PostProcessMap()
    {
        Map roomMap;
        List<Room> rooms;
        (roomMap, rooms) = mapGenerator.GenerateRooms(map);

        Map wallMap;
        List<Room> roomsWithWalls;
        (wallMap, roomsWithWalls) = mapGenerator.GenerateWalls(roomMap, rooms);
        
        delaunayTriangulation = new DelaunayTriangulation(roomsWithWalls);
        GetDelaunayEdges();

        return (wallMap, roomsWithWalls);
    }
    
    private void GetDelaunayEdges()
    {
        delaunayEdges = pointByPoint 
            ? delaunayTriangulation.GenerateDelaunayTriangulationPointByPoint() 
            : delaunayTriangulation.GenerateDelaunayTriangulationInstantly();
    }

    private void DisplayMap(List<Room> rooms)
    {
        mapDisplayer = new MapDisplayer(mapGenerator.Width,
            mapGenerator.Height,
            cellsParent,
            groundPrototypePrefab,
            midpointPrototypePrefab,
            roomInfoPanelParent,
            roomInfoPanelPrefab,
            wallPrototypePrefab,
            corridorPrototypePrefab,
            generationText);
        mapDisplayer.RemoveRoomInfoAndCells();

        mapDisplayer.LogMap(map, currentGeneration);
        if (rooms != null) mapDisplayer.LogRoomInfo(rooms);
    }

}