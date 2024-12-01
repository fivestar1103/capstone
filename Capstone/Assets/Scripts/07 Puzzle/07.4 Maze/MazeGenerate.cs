using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerate
{
    public int[,] GenerateMaze (int[,] map, List<(int, int)> entrances)
    {
        int[,] maze = (int[,]) map.Clone();
        int[,] visited = (int[,]) map.Clone();

        // 좌 우 하 상
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        int[] directionOrder = { 0, 1, 2, 3 };      // 탐색 방향 순서

        // 미로 가로, 세로 길이
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);

        // 미로 생성 시작점
        var startPos = (x: 0, y: 0);

        // 좌표 임시 저장할 변수들
        var nowPos = (x: 0, y: 0);
        var stackTopPos = (x: 0, y: 0);

        // DFS 스택
        var recursiveStack = new Stack<(int x, int y)>();
        recursiveStack.Push(startPos);
        visited[startPos.y, startPos.x] = 1;

        // Recursive Backtracking
        while(recursiveStack.Count != 0)
        {
            nowPos = recursiveStack.Pop();          // 현재 위치
            visited[nowPos.y, nowPos.x] = 1;
            
            ShuffleArray(directionOrder);           // 탐색 방향 순서 섞기

            for (int i = 0; i < 4; i++)
            {
                (int x, int y) visitPos = (nowPos.x + dx[directionOrder[i]]*2, nowPos.y + dy[directionOrder[i]]*2);

                // 탐색 중인 칸이 맵 범위 내에 있는지 확인
                if (0 <= visitPos.x && 0 <= visitPos.y && visitPos.x < mazeWidth && visitPos.y < mazeHeight)
                {
                    // 방문 여부 확인
                    if (visited[visitPos.y, visitPos.x] == 0)
                    {
                        recursiveStack.Push(nowPos);
                        recursiveStack.Push(visitPos);
                        break;
                    }
                }
            }

            // 방문 예정 스택 상단에, 새로 추가한 블럭이 아니라 이미 방문했던 블럭이 있다면 통로 만들어주기
            if (recursiveStack.Count != 0)
            {
                stackTopPos = recursiveStack.Peek();
                
                if (visited[stackTopPos.y, stackTopPos.x] == 1)
                {
                    (int x, int y) tempPos = ((nowPos.x + stackTopPos.x) / 2, (nowPos.y + stackTopPos.y) / 2);
                    
                    if (maze[tempPos.y, tempPos.x] != 3)
                        maze[tempPos.y, tempPos.x] = 0;
                }
            }
        }

        CreateFloorAroundEntrance(ref maze, entrances);

        return maze;
    }

    private void CreateFloorAroundEntrance(ref int[,] maze, List<(int, int)> entrances)
    {
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);

        // 각 문 출입구 근처 길 뚫어주기
        foreach ((int x, int y) entrancePos in entrances)
        {
            if (entrancePos.y % 2 == 0 && entrancePos.x % 2 == 0)
            {
                var visitPos = (x: 0, y: 0);

                if (entrancePos.x == (mazeWidth - 1))
                    visitPos = (entrancePos.x - 1, entrancePos.y);
                else if (entrancePos.y == (mazeHeight - 1))
                    visitPos = (entrancePos.x, entrancePos.y - 1);

                if (maze[visitPos.y, visitPos.x] != 3)
                    maze[visitPos.y, visitPos.x] = 0;
            }
        }
    }

    public void SelectInteractiveWalls(ref int[,] maze)
    {
        var mazeWidth = maze.GetLength(1);
        var mazeHeight = maze.GetLength(0);

        // Map의 구역을 4개로 나눠서 구역마다 버튼을 배치해야 함
        (int x, int y)[] maxList = { (mazeWidth/2, mazeHeight/2), (mazeWidth, mazeHeight/2), (mazeWidth/2, mazeHeight), (mazeWidth, mazeHeight) };
        (int x, int y)[] minList = { (0, 0), (mazeWidth/2 + 1, 0), (0, mazeHeight/2 + 1), (mazeWidth/ 2 + 1, mazeHeight/ 2 + 1) };

        for (int i = 0; i < 4; i++)
        {
            // wall 칸들을 리스트에 저장
            List<(int, int)> wallCells = new List<(int, int)>();
            (int x, int y) tempPos = (0, 0);

            for (int h = minList[i].y; h < maxList[i].y; h++)
            {
                for (int w = minList[i].x; w < maxList[i].x; w++)
                {
                    // 범위 내 && 상하좌우 중 통로 존재 && 선택된 칸이 벽이면 wallCells에 저장
                    if (CheckInRange(maze, (w, h)))
                    {
                        if (maze[h - 1, w] == 0 || maze[h, w - 1] == 0 || maze[h + 1, w] == 0 || maze[h, w + 1] == 0)
                        {
                            if (maze[h, w] == 1)
                            {
                                wallCells.Add((w, h));
                            }
                        }
                    }
                }
            }

            // 저장된 wall 칸들 중 랜덤으로 InteractiveWall로 변경
            tempPos = wallCells[Random.Range(0, wallCells.Count)];
            maze[tempPos.y, tempPos.x] = 2;
        }
    }

    private bool CheckInRange(int[,] maze, (int x, int y) nowPos)
    {
        bool inRangeFlag = true;
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            (int x, int y) tempPos = (nowPos.x + dx[i], nowPos.y + dy[i]);
            if (tempPos.x < 0 || tempPos.y < 0 || tempPos.x >= maze.GetLength(1) || tempPos.y >= maze.GetLength(0))
            {
                inRangeFlag = false;
                break;
            }
        }

        return inRangeFlag;
    }

    // Fisher-Yates Shuffle
    private void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var randomIndex = Random.Range(i, array.Length);
            Swap(array, i, randomIndex);
        }
    }

    private void Swap<T>(T[] array, int a, int b)
    {
        var temp = array[a];
        array[a] = array[b];
        array[b] = temp;
    }
}
