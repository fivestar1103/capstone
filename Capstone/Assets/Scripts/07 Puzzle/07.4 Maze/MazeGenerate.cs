using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerate
{
    public int[,] GenerateMaze (int[,] map, (int, int)[] doors)
    {
        int[,] maze = (int[,]) map.Clone();
        int[,] visited = (int[,])map.Clone();

        // 좌 우 하 상
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };
        int[] directionOrder = { 0, 1, 2, 3 };      // 탐색 방향 순서

        // 미로 가로, 세로 길이
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);

        // 미로 생성 시작점
        var startPos = (0, 0);

        // 좌표 임시 저장할 변수들
        var nowPos = (0, 0);
        var stackTopPos = (0, 0);

        // DFS 스택
        var recursiveStack = new Stack<(int, int)>();
        recursiveStack.Push(startPos);
        visited[startPos.Item2, startPos.Item1] = 1;

        // Recursive Backtracking
        while(recursiveStack.Count != 0)
        {
            nowPos = recursiveStack.Pop();      // 현재 위치
            visited[nowPos.Item2, nowPos.Item1] = 1;
            
            ShuffleArray(directionOrder);           // 탐색 방향 순서 섞기

            for (int i = 0; i < 4; i++)
            {
                var visitPos = (nowPos.Item1 + dx[directionOrder[i]]*2, nowPos.Item2 + dy[directionOrder[i]]*2);

                // 탐색 중인 칸이 맵 범위 내에 있는지 확인
                if (0 <= visitPos.Item1 && 0 <= visitPos.Item2 && visitPos.Item1 < mazeWidth && visitPos.Item2 < mazeHeight)
                {
                    // 방문 여부 확인
                    if (visited[visitPos.Item2, visitPos.Item1] == 0)
                    {
                        recursiveStack.Push(nowPos);
                        recursiveStack.Push(visitPos);
                        break;
                    }
                }
            }

            // 방문 예정 스택에 새로 추가한 것이 아니라 이전에 방문했던 블럭이 있다면 통로 만들어주기
            if (recursiveStack.Count != 0)
            {
                stackTopPos = recursiveStack.Peek();
                
                if (visited[stackTopPos.Item2, stackTopPos.Item1] == 1)
                {
                    var tempPos = ((nowPos.Item1 + stackTopPos.Item1) / 2, (nowPos.Item2 + stackTopPos.Item2) / 2);
                    maze[tempPos.Item2, tempPos.Item1] = 0;
                }
            }
        }
        
        // 각 문으로 향하는 부분 길 뚫어주기
        foreach (var doorPos in doors)
        {
            if ((mazeWidth % 2 == 0 || mazeHeight % 2 == 0) && maze[doorPos.Item2, doorPos.Item1] == 1)
            {
                var visitPos = (0, 0);
                maze[doorPos.Item2, doorPos.Item1] = 0;

                if (doorPos.Item1 == (mazeWidth - 1))
                    visitPos = (doorPos.Item1 - 1, doorPos.Item2);
                else if (doorPos.Item2 == (mazeHeight - 1))
                    visitPos = (doorPos.Item1, doorPos.Item2 - 1);

                maze[visitPos.Item2, visitPos.Item1] = 0;
            }
        }

        return maze;
    }

    public void SelectInteractiveWalls(ref int[,] maze)
    {
        var mazeWidth = maze.GetLength(1);
        var mazeHeight = maze.GetLength(0);

        // Map의 구역을 4개로 나눠서 구역마다 버튼을 배치해야 함
        (int, int)[] maxList = { (mazeWidth/2, mazeHeight/2), (mazeWidth, mazeHeight/2), (mazeWidth/2, mazeHeight), (mazeWidth, mazeHeight) };
        (int, int)[] minList = { (0, 0), (mazeWidth/2 + 1, 0), (0, mazeHeight/2 + 1), (mazeWidth/ 2 + 1, mazeHeight/ 2 + 1) };

        for (int i = 0; i < 4; i++)
        {
            // wall 칸들을 리스트에 저장
            List<(int, int)> wallCells = new List<(int, int)>();
            var tempPos = (0, 0);

            for (int h = minList[i].Item2; h < maxList[i].Item2; h++)
            {
                for (int w = minList[i].Item1; w < maxList[i].Item1; w++)
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
            maze[tempPos.Item2, tempPos.Item1] = 2;
        }
    }

    private bool CheckInRange(int[,] maze, (int, int) nowPos)
    {
        bool inRangeFlag = true;
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            var tempPos = (nowPos.Item1 + dx[i], nowPos.Item2 + dy[i]);
            if (tempPos.Item1 < 0 || tempPos.Item2 < 0 || tempPos.Item1 >= maze.GetLength(1) || tempPos.Item2 >= maze.GetLength(0))
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
