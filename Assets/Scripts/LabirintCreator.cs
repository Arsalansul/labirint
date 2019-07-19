﻿using System.Collections.Generic;
using UnityEngine;

public class LabirintCreator : MonoBehaviour
{
    // для простоты вычислений левый нижний угол лабиринта в точке 0, 0, 0
    private void CreateWalls(CellManager cellManager, int labirintSize)
    {
        GameObject walls = new GameObject("Walls");
        Vector3 position = new Vector3(0.5f, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);
        Vector3 deltaInRow = new Vector3(1, 0, 0);
        Vector3 deltaInColumn = new Vector3(-Settings.Instance.gameSettings.labirintSize, 0, 1);
        var cellDeltaPosition = new Vector3(0, 0, 0.5f);

        var wall = Cell.Wall.Down;
        for (int v = 0; v < 2; v++)
        {
            for (int i = 0; i < Settings.Instance.gameSettings.labirintSize + 1; i++)
            {
                for (int j = 0; j < Settings.Instance.gameSettings.labirintSize; j++)
                {
                    if (i == 0 || i == labirintSize)
                        Instantiate(Settings.Instance.wallSettings.WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                    else if (cellManager.GetCellByTransform(position + cellDeltaPosition).Walls.Contains(wall))
                    {
                        Instantiate(Settings.Instance.wallSettings.WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                    }
                    position += deltaInRow;
                }
                position += deltaInColumn;
            }

            position = new Vector3(0, 0, 0.5f);
            rotation = new Vector3(0, 90, 0);
            deltaInRow = new Vector3(0, 0, 1);
            deltaInColumn = new Vector3(1, 0, -Settings.Instance.gameSettings.labirintSize);
            wall = Cell.Wall.Left;
            cellDeltaPosition = new Vector3(0.5f, 0, 0);
        }
    }


    private void CreateLabirint(CellManager cellManager, int labirintSize)
    {
        List<Cell> unvisitedCells = new List<Cell>();
        for (int i = 0; i < labirintSize; i++)
        {
            for (int j = 0; j < labirintSize; j++)
            {
                unvisitedCells.Add(cellManager.cells[i, j]);
            }
        }

        Cell currentCell = cellManager.cells[Random.Range(0, labirintSize), Random.Range(0, labirintSize)];
        unvisitedCells.Remove(currentCell);

        Stack<Cell> path = new Stack<Cell>();
        while (unvisitedCells.Count > 0)
        {
            var randomUnvisitedNeighbour = cellManager.GetRandomNeighbourCellFromList(currentCell, unvisitedCells, labirintSize);
            if (randomUnvisitedNeighbour != null)
            {
                path.Push(currentCell);
                currentCell.CellCanMoveTo.Add(randomUnvisitedNeighbour);
                cellManager.RemoveWall(currentCell, randomUnvisitedNeighbour);
                currentCell = randomUnvisitedNeighbour;
                unvisitedCells.Remove(currentCell);
            }
            else if (path.Count > 0)
            {
                currentCell = path.Pop();
            }
            else
            {
                currentCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
                unvisitedCells.Remove(currentCell);
            }
        }
    }

    public void GenerateLab(CellManager cellManager, int labirintSize)
    {
        Destroy(GameObject.Find("Walls"));
        cellManager.CreateCells(labirintSize);
        CreateLabirint(cellManager, labirintSize);
        CreateWalls(cellManager, labirintSize);
    }
}