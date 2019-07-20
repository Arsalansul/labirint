using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    // для простоты вычислений левый нижний угол лабиринта в точке 0, 0, 0
    public Cell[,] cells;

    public void CreateCells(int labirintSize)
    {
        cells = new Cell[labirintSize, labirintSize];
        for (int i = 0; i < labirintSize; i++)
        {
            for (int j = 0; j < labirintSize; j++)
            {
                Cell cell = new Cell();
                cell.x = i;
                cell.y = j;
                SetWalls(cell);
                cells[i, j] = cell;
            }
        }
    }

    public Cell GetCellByTransform(Vector3 vector3)
    {
        return cells[(int)vector3.x, (int)vector3.z];
    }

    public Vector3 GetTransformByCell(Cell cell)
    {
        Vector3 vector = new Vector3(cell.x + 0.5f, 0.5f, cell.y + 0.5f);

        return vector;
    }

    public Cell GetRandomNeighbourCellContainedInList(Cell cell, List<Cell> cellsList, int labirintSize)
    {
        List<Cell> neighbourds = GetNeighbours(cell, labirintSize);
        while (neighbourds.Count > 0)
        {
            int index = Random.Range(0, neighbourds.Count);
            if (cellsList.Contains(neighbourds[index]))
            {
                return neighbourds[index];
            }
            neighbourds.RemoveAt(index);
        }

        return null;
    }

    private List<Cell> GetNeighbours(Cell cell, int labirintSize)
    {
        List<Cell> result = new List<Cell>();
        for (int i = -1; i < 2; i += 2)
        {
            if (cell.x + i >= 0 && cell.x + i < labirintSize)
            {
                result.Add(cells[cell.x + i, cell.y]);
            }
        }

        for (int i = -1; i < 2; i += 2)
        {
            if (cell.y + i >= 0 && cell.y + i < labirintSize)
            {
                result.Add(cells[cell.x, cell.y + i]);
            }
        }
        return result;
    }

    private void SetWalls(Cell cell)
    {
        cell.Walls = new List<Cell.Wall>
        {
            Cell.Wall.Top,
            Cell.Wall.Right,
            Cell.Wall.Bottom,
            Cell.Wall.Left
        };
    }

    public void RemoveWall(Cell cell1, Cell cell2)
    {
        if (cell1.x > cell2.x)
        {
            cell1.Walls.Remove(Cell.Wall.Left);
            cell2.Walls.Remove(Cell.Wall.Right);
        }
        else if (cell1.x < cell2.x)
        {
            cell1.Walls.Remove(Cell.Wall.Right);
            cell2.Walls.Remove(Cell.Wall.Left);
        }
        else if (cell1.y > cell2.y)
        {
            cell1.Walls.Remove(Cell.Wall.Bottom);
            cell2.Walls.Remove(Cell.Wall.Top);
        }
        else if (cell1.y < cell2.y)
        {
            cell1.Walls.Remove(Cell.Wall.Top);
            cell2.Walls.Remove(Cell.Wall.Bottom);
        }
        else
        {
            Debug.Log("equal cells");
        }
    }

    public void RemoveWall(Cell cell, Cell.Wall wall)
    {
        switch (wall)
        {
            case Cell.Wall.Top:
                if (cell.y == Settings.Instance.gameSettings.labirintSize-1) break;
                cell.Walls.Remove(wall);
                cells[cell.x, cell.y + 1].Walls.Remove(Cell.Wall.Bottom);
                break;
            case Cell.Wall.Right:
                if (cell.x == Settings.Instance.gameSettings.labirintSize - 1) break;
                cell.Walls.Remove(wall);
                cells[cell.x+1, cell.y].Walls.Remove(Cell.Wall.Left);
                break;
            case Cell.Wall.Bottom:
                if (cell.y == 0) break;
                cell.Walls.Remove(wall);
                cells[cell.x, cell.y - 1].Walls.Remove(Cell.Wall.Top);
                break;
            case Cell.Wall.Left:
                if (cell.x == 0) break;
                cell.Walls.Remove(wall);
                cells[cell.x - 1, cell.y].Walls.Remove(Cell.Wall.Right);
                break;
        }
    }

    public List<Cell> GetPassableNeighbours(Cell cell, int labirintSize)
    {
        var result = GetNeighbours(cell, labirintSize);

        foreach (var wall in cell.Walls)
        {
            if (wall == Cell.Wall.Top && cell.y <labirintSize - 1)
            {
                result.Remove(cells[cell.x, cell.y + 1]);
            }
            else if (wall == Cell.Wall.Right && cell.x < labirintSize - 1)
            {
                result.Remove(cells[cell.x +1, cell.y]);
            }
            else if (wall == Cell.Wall.Bottom && cell.y > 0)
            {
                result.Remove(cells[cell.x, cell.y - 1]);
            }
            else if (wall == Cell.Wall.Left && cell.x > 0)
            {
                result.Remove(cells[cell.x - 1, cell.y]);
            }
        }

        return result;
    }
}
