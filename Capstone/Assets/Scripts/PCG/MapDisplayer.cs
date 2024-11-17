using System;
using System.Collections.Generic;
using System.Linq;
using PCG.Data_Structures;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MapDisplayer
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public GameObject CellsParent { get; private set; }
    public GameObject CellPrototypePrefab { get; private set; }
    public GameObject RoomInfoPanelParent { get; private set; }
    public GameObject RoomInfoPanelPrefab { get; private set; }
    public TextMeshProUGUI GenerationText { get; private set; }
    
    public MapDisplayer(int width,
        int height,
        GameObject cellsParent,
        GameObject cellPrototypePrefab,
        GameObject roomInfoPanelParent,
        GameObject roomInfoPanelPrefab,
        TextMeshProUGUI generationText)
    {
        Width = width;
        Height = height;
        CellsParent = cellsParent;
        CellPrototypePrefab = cellPrototypePrefab;
        RoomInfoPanelParent = roomInfoPanelParent;
        RoomInfoPanelPrefab = roomInfoPanelPrefab;
        GenerationText = generationText;
    }

    public void RemoveRoomInfoAndCells()
    {
        foreach (Transform child in RoomInfoPanelParent.transform)
            GameObject.Destroy(child.gameObject);
        
        foreach (Transform child in CellsParent.transform)
            GameObject.Destroy(child.gameObject);
    }
    
    public void LogRoomInfo(List<Room> rooms)
    {
        GenerationText.text += $"\nTotal rooms: {rooms.Count}";
        foreach (var room in rooms)
        {
            var roomInfoPanel = GameObject.Instantiate(RoomInfoPanelPrefab, RoomInfoPanelParent.transform);
            roomInfoPanel.name = $"Room {room.RoomNumber}";
            
            var roomInfoText = roomInfoPanel.GetComponentInChildren<TextMeshProUGUI>();
            roomInfoText.text = $"Room {room.RoomNumber}:" +
                                $"\tCenter: ({room.CenterCell.X}, {room.CenterCell.Y})\n" +
                                
                                $"\tSize: {room.RoomCells.Count}" +
                                $"\tTop Left: ({room.X}, {room.Y})\n" +
                                
                                $"\tWidth: {room.Width}" +
                                $"\tHeight: {room.Height}";
            
            // log each cell in room
            // Debug.Log($"Room {room.RoomNumber}\nRoom Cells: {string.Join(", ", room.RoomCellsRelative.Select(cell => $"({cell.X}, {cell.Y})"))}\nWall Cells: {string.Join(", ", room.WallCellsRelative.Select(cell => $"({cell.X}, {cell.Y})"))}");
        }
    }

    public void LogMap(Map map, int currentGeneration)
    {
        var cellSize = CellPrototypePrefab.GetComponent<RectTransform>().rect.width;
        var screenTopLeft = new Vector3(-Width * cellSize / 2, Height * cellSize / 2, 0);
        
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                var cell = map.Cells[y, x];
                var cellType = cell.Type;
                if (cellType == CellType.Blank) continue;

                var cellObject = GameObject.Instantiate(CellPrototypePrefab, CellsParent.transform);
                cellObject.name = $"Cell ({x}, {y})";
                // var vector3 = new Vector3(x * cellSize, - y * cellSize, 0);
                var position = screenTopLeft + new Vector3(x * cellSize, -y * cellSize, 0);
                cellObject.transform.localPosition = position;

                // Debug.Log($"Cell ({x}, {y}) Type: {cellType}");
                switch (cellType)
                {
                    // case CellType.Blank:
                    // {
                    //     cellObject.GetComponent<SpriteRenderer>().color = Color.white;
                    //     break;
                    // }
                    case CellType.Room:
                    {
                        if (cell is not RoomCell) continue;
                        cellObject.GetComponent<SpriteRenderer>()
                            .color = ((RoomCell)cell).IsCenter
                            ? Color.yellow
                            : Color.blue;
                        var roomNumberText = cellObject.GetComponentInChildren<TextMeshPro>();
                        roomNumberText.text = ((RoomCell)cell).RoomNumber.ToString();
                        break;
                    }
                    case CellType.Wall:
                    {
                        cellObject.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                    }
                    case CellType.Corridor:
                    {
                        cellObject.GetComponent<SpriteRenderer>().color = Color.green;
                        break;
                    }
                    default:
                        break;
                }
            }
        
        GenerationText.text = $"Generation {currentGeneration}";
    }
    
    public void LogMapText(Map rawMap)
    {
        var lines = "";
        for (var y = 0; y < Height; y++)
        {
            var line = "";
            for (var x = 0; x < Width; x++)
            {
                string roomType;
                switch (rawMap.Cells[y, x].Type)
                {
                    case CellType.Blank:
                        roomType = "-";
                        break;
                    case CellType.Room:
                        roomType = "R";
                        break;
                    case CellType.Wall:
                        roomType = "W";
                        break;
                    case CellType.Corridor:
                        roomType = "C";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                line += roomType;
            }

            lines += line + "\n";
        }
        
        Debug.Log(lines);
    }
}
