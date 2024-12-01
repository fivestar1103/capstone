using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerate : MonoBehaviour
{
    [SerializeField] private GameObject TileMap;
    [SerializeField] private List<GameObject> prefabList;

    enum Blocks { NONE = -1, FLOOR = 0, WALL, INTERACTIVE, ENTRANCE }

    private void Awake()
    {
        MazeManager.mapGenerate += GenerateMap;
    }

    // public void GenerateMap(int[,] maze, Room mazeRoom)
    public void GenerateMap(int[,] maze)
    {
        foreach (Transform child in TileMap.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject prefab;
        var nowPos = Blocks.NONE;
        //int absolutePosX = mazeRoom.X;
        //int absolutePosY = mazeRoom.Y;
        int absolutePosX = 0;
        int absolutePosY = 0;

        for (int height = 0;  height < maze.GetLength(0); height++)
        {
            for (int width = 0; width < maze.GetLength(1); width++)
            {
                nowPos = (Blocks)maze[height, width];

                switch (nowPos)
                {
                    case Blocks.NONE:
                        break;
                    case Blocks.FLOOR:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(absolutePosY + height * 4, 0, absolutePosX + width * 4);
                        break;
                    case Blocks.WALL:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(absolutePosY + height * 4, 0, absolutePosX + width * 4);
                        break;
                    case Blocks.INTERACTIVE:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(absolutePosY + height * 4, 0, absolutePosX + width * 4);

                        Debug.Log(prefab.transform.localPosition);
                        Debug.Log(maze[height + 1, width]);
                        if (maze[height + 1, width] == 1)
                        {
                            Quaternion IWRotation = RotateInteractiveWalls(maze, (width, height));
                            prefab.transform.rotation = IWRotation;
                        }
                        Debug.Log(prefab.transform.rotation);
                        break;
                    case Blocks.ENTRANCE:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(height * 4, 0, width * 4);
                        break;
                }
                
            }
        }
    }

    private Quaternion RotateInteractiveWalls(int[,] maze, (int, int) IWPos)
    {
        Quaternion rotation = Quaternion.identity;

        // аб ╩С ©Л
        int[] dx = { -1, 0, 1 };
        int[] dy = { 0, -1, 0 };
        int[] directionOrder = { 0, 1, 2 };

        ShuffleArray(directionOrder);

        for (int i = 0; i < 3; i++)
        {
            var tempPos = (IWPos.Item1 + dx[directionOrder[i]], IWPos.Item2 + dy[directionOrder[i]]);
            if (0 <= tempPos.Item1 && 0 <= tempPos.Item2 && tempPos.Item1 < maze.GetLength(1) && tempPos.Item2 < maze.GetLength(0))
            {
                if (maze[tempPos.Item2, tempPos.Item1] == 0)
                {
                    rotation = Quaternion.Euler(0, (directionOrder[i] + 1) * 90, 0);
                }
            }
        }

        return rotation;
    }

    // Fisher-Yates Shuffle
    private void ShuffleArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var randomIndex = Random.Range(i, array.Length);
            // Swap
            var temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
