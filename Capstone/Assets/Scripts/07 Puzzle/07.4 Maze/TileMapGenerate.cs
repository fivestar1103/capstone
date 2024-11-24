using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerate : MonoBehaviour
{
    [SerializeField] private GameObject TileMap;
    [SerializeField] private List<GameObject> prefabList;

    enum Blocks { NONE = -1, FLOOR = 0, WALL, INTERACTIVE, DOOR }

    private void Awake()
    {
        MazeManager.mapGenerate += GenerateMap;
    }

    public void GenerateMap(int[,] maze)
    {
        foreach (Transform child in TileMap.transform)
        {
            Destroy(child.gameObject);
        }

        GameObject prefab;
        var nowPos = Blocks.NONE;

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
                        prefab.transform.localPosition = new Vector3(1 + height * 2, -0.5f, 1 + width * 2);
                        break;
                    case Blocks.WALL:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(1 + height * 2, 3, 1 + width * 2);
                        break;
                    case Blocks.INTERACTIVE:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(1 + height * 2, 3, 1 + width * 2);
                        break;
                    case Blocks.DOOR:
                        prefab = Instantiate(prefabList[(int)nowPos], TileMap.transform);
                        prefab.transform.localPosition = new Vector3(1 + height * 2, 3, 1 + width * 2);
                        // prefab.transform.rotation = Quaternion.identity;
                        break;
                }
                
            }
        }
    }

    private void RotateInteractiveWalls(GameObject IWPrefab)
    {

    }
}
