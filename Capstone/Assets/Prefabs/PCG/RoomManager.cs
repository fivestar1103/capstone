using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager: MonoBehaviour
{
    // a singleton class to manage the rooms
    private static RoomManager _instance;
    public static RoomManager Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<RoomManager>();

            return _instance;
        }
    }

    public CellType CurrentCellType { get; set; }
    public int CurrentRoomNumber { get; set; }
    
    // create adjacency list for rooms
    public Dictionary<int, List<int>> AdjacencyList { get; set; }
    
    public void AddEdge(int u, int v)
    {
        if (!AdjacencyList.ContainsKey(u))
            AdjacencyList[u] = new List<int>();
        
        AdjacencyList[u].Add(v);
    }
    
    // log the adjacency list
    public void LogAdjacencyList()
    {
        foreach (var key in AdjacencyList.Keys)
        {
            var str = $"Room {key}: ";
            foreach (var value in AdjacencyList[key])
            {
                str += $"{value}, ";
            }
            Debug.Log(str);
        }
    }
    
}
