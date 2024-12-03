using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private MazeGenerate mazeGenerate;
    private ConvertToMap convertToMap;

    // public static Action<int[,], Room> mapGenerate;
    public static Action<int[,]> mapGenerate;

    // we need to select room for maze & bring it here
    // private Room mazeRoom;
    // ---------------------------------------------------
    private GameObject player;
    private int[,] maze;
    private List<(int x, int y)> entrances;
    private int mazeWidth;
    private int mazeHeight;

    private void Awake()
    {
        convertToMap = new ConvertToMap();
        mazeGenerate = new MazeGenerate();
        mazeWidth = 12;
        mazeHeight = 15;

        // we need to select room for maze & bring it here
        // mazeRoom = GetComponent<Room>();
        // convertToMap.ConvertDataStructure(mazeRoom, ref maze);
        // ---------------------------------------------------
        player = Instantiate(playerPrefab);
    }

    private void Start()
    {
        // 15 x 12 (행 x 열) -> C/C++이랑 순서 같음
        // maze = new int[mazeHeight, mazeWidth];

        // 15 x 12 (행 x 열) -> C/C++이랑 순서 같음
        maze = new int[,]
        {
            { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { -1, -1, -1, 0, 0, 0, 0, 0, -1, -1, 0, 3 },
            { -1, -1, -1, 0, 0, 0, 0, 0, -1, -1, 0, 0 },
            { -1, -1, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1 },
            { -1, -1, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1 },
        };
        mazeWidth = maze.GetLength(1);
        mazeHeight = maze.GetLength(0);

        //// 현재 반례
        //maze = new int[,]
        //{
        //    { 3, 0, -1, 0, 0, 0, 0, 0, 0, 0, -1, -1 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1 },
        //    { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, -1, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { -1, -1, -1, 0, 0, 0, 0, 0, -1, -1, 0, 3 },
        //    { -1, -1, -1, 0, 0, 0, 0, 0, -1, -1, 0, 0 },
        //    { -1, -1, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1 },
        //    { -1, -1, 0, 0, 0, 0, 0, 0, -1, -1, -1, -1 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1 },
        //};
        //mazeWidth = maze.GetLength(1);
        //mazeHeight = maze.GetLength(0);

        entrances = new List<(int x, int y)>();
        FindDoors(maze);

        convertToMap.DebugPrintMap(maze);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            InitializeMaze();

            Debug.Log("GenerateMaze!");
            maze = mazeGenerate.GenerateMaze(maze, entrances);
            mazeGenerate.SelectInteractiveWalls(ref maze);

            // mapGenerate(maze, mazeRoom);
            mapGenerate(maze);

            // GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(1 + entrances[0].y * 2, 1.5f, 1 + entrances[0].x * 2);

            convertToMap.DebugPrintMap(maze);
        }
    }

    private void InitializeMaze()
    {
        for (int i = 0; i < mazeHeight; i++)
        {
            for (int j = 0; j < mazeWidth; j++)
            {
                // Floor인 경우에만
                if (maze[i, j] == 0)
                {
                    // 행과 열마다 홀수 인덱스 칸만 벽으로 변경
                    if (i % 2 == 0 && j % 2 == 0)
                        maze[i, j] = 0;
                    else
                        maze[i, j] = 1;
                }
            }
        }
    }

    private void FindDoors(int[,] maze)
    {
        for (int h = 0;  h < mazeHeight; h++)
        {
            for (int w = 0;  w < mazeWidth; w++)
            {
                if (maze[h, w] == 3)
                    entrances.Add((x: w, y: h));
            }
        }
    }
}
