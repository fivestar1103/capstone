using System.Collections.Generic;
using PCG.Data_Structures;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float RoomPercentage { get; private set; }
    public int Generations { get; private set; }
    public int BirthLimit { get; private set; }
    public int DeathLimit { get; private set; }
    public int RoomThreshold { get; private set; }

    public MapGenerator(int width,
        int height,
        float roomPercentage,
        int generations,
        int birthLimit,
        int deathLimit,
        int roomThreshold)
    {
        Width = width;
        Height = height;
        RoomPercentage = roomPercentage;
        Generations = generations;
        BirthLimit = birthLimit;
        DeathLimit = deathLimit;
        RoomThreshold = roomThreshold;
    }

    public Map GenerateMap()
    {
        // create a new map
        var rawMap = new Map(Width, Height);

        // choose random room cells and set them to room type
        var totalCells = Width * Height;
        var roomCells = (int)(totalCells * RoomPercentage);
        while (roomCells > 0)
        {
            var x = Random.Range(0, Width);
            var y = Random.Range(0, Height);
            if (rawMap.Cells[y, x].Type == CellType.Room) continue;

            rawMap.Cells[y, x].Type = CellType.Room;
            roomCells--;
        }

        // run cellular automata for a number of generations
        for (var i = 0; i < Generations; i++)
            CellularAutomata(rawMap); 

        return rawMap;
    }
    
    public (Map, List<Room>) GenerateRooms(Map map)
    {
        var rooms = new List<Room>();
        var roomNumber = 0;
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                if (map.Cells[y, x].Type != CellType.Room || map.Cells[y, x] is RoomCell) continue;
                
                var room = LabelRoomWithBFS(x, y, map, roomNumber);
                if (room.RoomCells.Count < RoomThreshold)
                {
                    foreach (var roomCell in room.RoomCells)
                        map.Cells[roomCell.Y, roomCell.X].Type = CellType.Blank;
                }
                else
                {
                    rooms.Add(room);
                    roomNumber++;
                }
            }
        
        return (map, rooms);
    }

    // use a BFS algorithm to label each room cell with a unique room number
    private Room LabelRoomWithBFS(int startX, int startY, Map map, int roomNumber)
    {
        var room = new Room(roomNumber);
        var visited = new bool[Height, Width];
        var queue = new Queue<Cell>();
        
        visited[startY, startX] = true;
        queue.Enqueue(map.Cells[startY, startX]);
        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            var roomCell = new RoomCell(cell.X, cell.Y, CellType.Room, roomNumber); 
            map.Cells[cell.Y, cell.X] = roomCell;
            room.AddCell(roomCell);
                    
            var neighbours = GetNeighbours(map, cell.X, cell.Y, adjacent:true);
            foreach (var neighbour in neighbours)
                if (neighbour.Type == CellType.Room && !visited[neighbour.Y, neighbour.X])
                {
                    queue.Enqueue(neighbour);
                    visited[neighbour.Y, neighbour.X] = true;
                }
        }
        
        return room;
    }

    private void CellularAutomata(Map map)
    {
        var birthCount = 0;
        var deathCount = 0;
        
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var neighboursCount = GetNeighbours(map, x, y).Count;
                switch (map.Cells[y, x].Type)
                {
                    case CellType.Blank:
                    {
                        if (neighboursCount > BirthLimit)
                        {
                            map.Cells[y, x].Type = CellType.Room;
                            birthCount++;
                        }
                                
                        break;
                    }
                    case CellType.Room:
                    {
                        if (neighboursCount < DeathLimit)
                        {
                            map.Cells[y, x].Type = CellType.Blank;
                            deathCount++;
                        }
                                
                        break;
                    }
                    case CellType.Wall:
                    case CellType.Corridor:
                    default:
                        break;
                }
            }
        
        // Debug.Log($"Births: {birthCount}, Deaths: {deathCount}");
    }

    public Map GenerateWalls(Map map, List<Room> rooms)
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (map.Cells[y, x].Type == CellType.Blank && GetNeighbours(map, x, y).Count > 0)
                    map.Cells[y, x].Type = CellType.Wall;

        var deletedRoomCells = new List<RoomCell>();
        foreach (var room in rooms)
            foreach (var roomCell in room.RoomCells)
                if (IsEdgeCellPosition(roomCell.X, roomCell.Y))
                {
                    map.Cells[roomCell.Y, roomCell.X].Type = CellType.Wall;
                    deletedRoomCells.Add(roomCell);
                }
        foreach (var roomCell in deletedRoomCells)
            rooms[roomCell.RoomNumber].DeleteCell(roomCell);

        return map;
    }

    private static bool IsValidCellPosition(Map map, int x, int y) // check if a given cell position is valid
    {
        return x >= 0 && y >= 0 && x < map.Width && y < map.Height;
    }

    private bool IsEdgeCellPosition(int x, int y)
    {
        return x == 0 || x == Width - 1 || y == 0 || y == Height - 1;
    }
    
    private static List<Cell> GetNeighbours(Map map, int x, int y, bool adjacent=false) // get the room cells around a given cell
    {
        var neighbours = new List<Cell>();
        for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) || (adjacent && i * j != 0)) 
                    continue;

                var neighbourX = x + i;
                var neighbourY = y + j;
                if (IsValidCellPosition(map, neighbourX, neighbourY) &&
                    map.Cells[neighbourY, neighbourX].Type == CellType.Room)
                    neighbours.Add(map.Cells[neighbourY, neighbourX]);
            }

        return neighbours;
    }
}
