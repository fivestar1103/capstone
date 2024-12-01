using PCG.Data_Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConvertToMap
{
    public void ChoiceMazeRoom()
    {
        // Room의 리스트 전체를 받아서 미로가 될 만한 room을 골라야 하나?
    }

    public void ConvertDataStructure(Room inRoom, ref int[,] roomCellsArray)
    {
        Debug.Log($"inRoom height, width : [{inRoom.Height}, {inRoom.Width}]\n" +
            $"roomCellsArray height, width : [{roomCellsArray.GetLength(0)}, {roomCellsArray.GetLength(1)}]");

        foreach (var roomCell in inRoom.RoomCellsRelative)
        {
            roomCellsArray[roomCell.Y, roomCell.X] = 0;
        }

        foreach (var Cell in inRoom.WallCellsRelative)
        {
            //if (0 <= Cell.Y && Cell.Y < inRoom.Height && 0 <= Cell.X && Cell.X < inRoom.Width)
            //    Debug.Log($"in of range y, x : [{Cell.Y}, {Cell.X}]");
            //else if (0 == Cell.Y || 0 == Cell.X)
            //    Debug.Log($"out of range y, x : [{Cell.Y}, {Cell.X}]");
            //else if (Cell.Y >= inRoom.Height || Cell.X >= inRoom.Width)
            //    Debug.Log($"out of range y, x : [{Cell.Y}, {Cell.X}]");

            roomCellsArray[Cell.Y, Cell.X] = 1;
        }

        foreach (var Cell in inRoom.CorridorCellsRelative)
        {
            roomCellsArray[Cell.Y, Cell.X] = 3;
        }
    }

    public void DebugPrintMap<T>(T[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        string output = "[2D Array Contents]:\n";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                output += array[i, j] + "\t";
            }
            output += "\n";
        }
        Debug.Log(output);
    }
}
