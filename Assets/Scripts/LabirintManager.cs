using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class LabirintManager
    {
        private readonly CellManager cellManager;

        public LabirintManager(CellManager _cellManager)
        {
            cellManager = _cellManager;
        }

        public void CreateLabirint()
        {
            var cells = cellManager.cells;
            cellManager.Visited(0);

            var unvisitedCellsCount = cells.Length - 1;

            var currentCellIndex = 0;

            Stack<int> path = new Stack<int>();
            while (unvisitedCellsCount > 0)
            {
                if ((cells[currentCellIndex] & CellManager.maskAllNeighbours) != 0)
                {
                    path.Push(currentCellIndex);
                    int randomUnvisitedNeghbourIndex = currentCellIndex + cellManager.GetRandomUnvisitedNeghbourIndex(cells[currentCellIndex]);
                    cellManager.RemoveWall(currentCellIndex, randomUnvisitedNeghbourIndex);
                    currentCellIndex = randomUnvisitedNeghbourIndex;
                    cellManager.Visited(currentCellIndex);
                    unvisitedCellsCount--;
                }
                else //if (path.Count > 0)
                {
                    currentCellIndex = path.Pop();
                }
            }
        }

    }
}