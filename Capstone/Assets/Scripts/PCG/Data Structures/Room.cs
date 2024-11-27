using System;
using System.Collections.Generic;
using System.Linq;
using PCG.Data_Structures;
using UnityEngine;

public class Room
{
    public int RoomNumber { get; set; }
    public List<RoomCell> RoomCells { get; private set; }
    public List<Cell> WallCells { get; private set; }
    public List<RoomCell> RoomCellsRelative { get; set; }
    public List<Cell> WallCellsRelative { get; set; }
    public int X { get; set; } // top left corner including walls
    public int Y { get; set; } // top left corner including walls
    public int Width { get; set; } // width including walls
    public int Height { get; set; } // height including walls
    public RoomCell CenterCell { get; set; }
    
    public Room(int roomNumber)
    {
        RoomNumber = roomNumber;
        RoomCells = new List<RoomCell>();
        WallCells = new List<Cell>();
        RoomCellsRelative = new List<RoomCell>();
        WallCellsRelative = new List<Cell>();
    }
    
    public void AddCell(Cell cell) // cell position should be absolute
    {
        switch (cell.Type)
        {
            case CellType.Wall:
                WallCells.Add(cell);
                WallCellsRelative.Add(new Cell(cell.X - X, cell.Y - Y, CellType.Wall));
                break;
            case CellType.Room:
                RoomCells.Add((RoomCell)cell);
                break;
            case CellType.Blank:
            case CellType.Corridor:
            default:
                break;
        }
    }
    
    public void DeleteCell(Cell cell)
    {
        switch (cell.Type)
        {
            case CellType.Wall:
                WallCells.Remove(cell);
                break;
            case CellType.Room:
                RoomCells.Remove((RoomCell)cell);
                break;
            case CellType.Blank:
            case CellType.Corridor:
            default:
                break;
        }
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
        Width = rightMostIndex - leftMostIndex + 1;
        Height = bottomMostIndex - topMostIndex + 1;
        
        var centerX = Mathf.FloorToInt((float)xSum / RoomCells.Count); 
        var centerY = Mathf.FloorToInt((float)ySum / RoomCells.Count);
        var centerCell = RoomCells.OrderBy(cell => Mathf.Abs(cell.X - centerX) + Mathf.Abs(cell.Y - centerY))
            .FirstOrDefault(); // get the cell closest to the center of the room

        centerCell.IsCenter = true;
        CenterCell = centerCell;
        
        CalculateRoomCellsRelative();
    }
    
    public void CalculateRoomCellsRelative()
    {
        foreach (var roomCell in RoomCells)
        {
            var relativeX = roomCell.X - X;
            var relativeY = roomCell.Y - Y;
            RoomCellsRelative.Add(new RoomCell(relativeX,
                relativeY,
                roomNumber: roomCell.RoomNumber,
                isCenter: roomCell.IsCenter));
        }
    }
}
