using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    // when pressed(collided) by player, change the current room value in the RoomManager
    
    public CellType CellType;
    public int RoomNumber;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RoomManager.Instance.CurrentCellType = CellType;
            var str = $"Current Cell Type: {RoomManager.Instance.CurrentCellType}";
            
            if (CellType == CellType.Room)
            {
                RoomManager.Instance.SetCurrentRoomNumber(RoomNumber);
                str += $"\nCurrent Room Number: {RoomManager.Instance.CurrentRoomNumber}\");"; 
            }

            Debug.Log(str);
        }
    }
}
