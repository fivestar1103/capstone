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
            if (_instance == null)
                _instance = FindObjectOfType<RoomManager>();

            return _instance;
        }
    }

    public CellType CurrentCellType { get; set; }
    public int CurrentRoomNumber { get; set; }
}
