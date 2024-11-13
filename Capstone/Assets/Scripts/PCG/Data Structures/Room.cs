using System;
using System.Collections.Generic;
using System.Linq;
using PCG.Data_Structures;
using UnityEngine;

public class Room
{
    public int RoomNumber { get; set; }
    public List<RoomCell> RoomCells { get; set; }
    public int X { get; set; } // top left corner including walls
    public int Y { get; set; } // top left corner including walls
    public int Width { get; set; } // width including walls
    public int Height { get; set; } // height including walls
    public RoomCell CenterCell { get; set; }
    
    public Room(int roomNumber)
    {
        RoomNumber = roomNumber;
        RoomCells = new List<RoomCell>();
    }
    
    public void AddCell(RoomCell cell)
    {
        RoomCells.Add(cell);
    }
    
    public void DeleteCell(RoomCell cell)
    {
        RoomCells.Remove(cell);
    }

    public void CalculateRoomInfo()
    {
        var leftMostIndex = int.MaxValue;
        var rightMostIndex = int.MinValue;
        var topMostIndex = int.MaxValue;
        var bottomMostIndex = int.MinValue;
        
        var xSum = 0;
        var ySum = 0;
        
        foreach (var roomCell in RoomCells)
        {
            if (roomCell.X < leftMostIndex)
                leftMostIndex = roomCell.X;
            if (roomCell.X > rightMostIndex)
                rightMostIndex = roomCell.X;
            if (roomCell.Y < topMostIndex)
                topMostIndex = roomCell.Y;
            if (roomCell.Y > bottomMostIndex)
                bottomMostIndex = roomCell.Y;
            
            xSum += roomCell.X;
            ySum += roomCell.Y;
        }
        
        X = leftMostIndex - 1;
        Y = topMostIndex - 1;
        Width = rightMostIndex - leftMostIndex + 2;
        Height = bottomMostIndex - topMostIndex + 2;
        
        var centerX = Mathf.FloorToInt((float)xSum / RoomCells.Count); 
        var centerY = Mathf.FloorToInt((float)ySum / RoomCells.Count);
        var centerCell = RoomCells.OrderBy(cell => Mathf.Abs(cell.X - centerX) + Mathf.Abs(cell.Y - centerY))
            .FirstOrDefault(); // get the cell closest to the center of the room

        CenterCell = centerCell;
        centerCell.IsCenter = true;
    }
}
