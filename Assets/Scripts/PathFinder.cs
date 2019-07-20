using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public List<Cell> openList = new List<Cell>();
    public List<Cell> closedList = new List<Cell>();

    private CellManager cellManager = CellManager.Instance;
    
    //ищим соседние ячейки через которые можем пройти НЕ из opneList

    private List<Cell> RemoveCellsContoinedInOpenList(List<Cell> list)
    {
        var result = new List<Cell>();
        result = list;
        for (int i =0; i< result.Count ; i++)
        {
            if (openList.Contains(result[i]))
                result.Remove(result[i]);
        }

        return result;
    }

    private int CellDist(Cell cell_1, Cell cell_2)
    {
        int result = 0;
        result += Mathf.Abs(cell_2.x - cell_1.x);
        result += Mathf.Abs(cell_2.y - cell_1.y);

        return result;
    }

    private void GetClosedList(Cell currentCell, Cell endCell)
    {
        closedList.Clear();
        openList.Clear();

        openList.Add(currentCell);
        currentCell.g = 0;
        currentCell.h = CellDist(currentCell, endCell);
        currentCell.f = currentCell.g + currentCell.h;

        while (openList.Count > 0)
        {
            currentCell = openList[0];

            openList.Remove(currentCell);
            closedList.Add(currentCell);

            if (currentCell == endCell)
                break;

            var passableNeighbours = cellManager.GetPassableNeighbours(currentCell);
            foreach (var cell in RemoveCellsContoinedInOpenList(passableNeighbours))
            {
                int tentativeScore = currentCell.g + 1; //distance from start to the neighbor through current
                if (closedList.Contains(cell) && tentativeScore >= cell.g) continue;
                cell.h = CellDist(cell, endCell);
                cell.g = tentativeScore;
                cell.f = cell.g + cell.h;
                cell.CameFrom = currentCell;
                if (!openList.Contains(cell))
                    openList.Add(cell);
            }
            openList.Sort((x, y) => x.f.CompareTo(y.f));
        }
    }

    public List<Cell> GetPath(Cell start, Cell target)
    {
        GetClosedList(start, target);

        if (closedList.Contains(target))
        {
            var result = new List<Cell>();
            var current = target;
            result.Add(current);
            while (current != start)
            {
                current = current.CameFrom;
                result.Add(current);
            }
            result.Reverse();
            result.Remove(start);
            return result;
        }

        Debug.Log("path does not exist");
        return null;
    }
}