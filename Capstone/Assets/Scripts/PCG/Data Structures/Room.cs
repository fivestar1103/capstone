using System;
using System.Collections.Generic;
using System.Linq;
using PCG.Data_Structures;
using UnityEngine;

public enum RoomType
{
    Battle,
    Puzzle
}

public class Room
{
    public int RoomNumber { get; set; }
    public RoomType Type { get; set; }
    
    public List<RoomCell> RoomCells { get; private set; }
    public List<Cell> WallCells { get; private set; }
    public List<Cell> CorridorCells { get; private set; }
    
    public List<RoomCell> RoomCellsRelative { get; set; }
    public List<Cell> WallCellsRelative { get; set; }
    public List<Cell> CorridorCellsRelative { get; set; }
    
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
        CorridorCells = new List<Cell>();
        RoomCellsRelative = new List<RoomCell>();
        WallCellsRelative = new List<Cell>();
        CorridorCellsRelative = new List<Cell>();
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
    
    public void LogRoomInfo()
    {
        // Create a 2D array representing the room including walls
        var display = new char[Height + 2, Width + 2];
    
        // Initialize with empty spaces
        for (int y = 0; y < Height + 2; y++)
        {
            for (int x = 0; x < Width + 2; x++)
            {
                display[y, x] = ' ';
            }
        }
    
        // Add room cells
        foreach (var cell in RoomCells)
        {
            var relativeX = cell.X - X;
            var relativeY = cell.Y - Y;
            display[relativeY, relativeX] = cell.IsCenter ? 'M' : 'R';
        }
    
        // Add wall cells
        foreach (var cell in WallCells)
        {
            var relativeX = cell.X - X;
            var relativeY = cell.Y - Y;
            display[relativeY, relativeX] = 'W';
        }
    
        // Add corridor cells
        foreach (var cell in CorridorCells)
        {
            var relativeX = cell.X - X;
            var relativeY = cell.Y - Y;
            display[relativeY, relativeX] = 'C';
        }
        
        var roomLayout = "\n";
    
        // Add top border
        roomLayout += "+";
        for (int x = 0; x < Width + 2; x++)
            roomLayout += "-";
        roomLayout += "+\n";
    
        // Add room content with side borders
        for (int y = 0; y < Height + 2; y++)
        {
            roomLayout += "|";
            for (int x = 0; x < Width + 2; x++)
            {
                roomLayout += display[y, x];
            }
            roomLayout += "|\n";
        }
    
        // Add bottom border
        roomLayout += "+";
        for (int x = 0; x < Width + 2; x++)
            roomLayout += "-";
        roomLayout += "+\n";
    
        // Print room information
        Debug.Log($"Room #{RoomNumber} - Type: {Type}\n" +
                  $"Position: ({X}, {Y}), Size: {Width}x{Height}\n" +
                  $"Center Cell: ({CenterCell.X}, {CenterCell.Y})\n" +
                  "Room Layout (W=Wall, R=Room, M=Center, C=Corridor):\n" + 
                  $"{roomLayout}");
    }
}
