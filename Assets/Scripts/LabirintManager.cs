﻿using UnityEngine;

namespace Assets.Scripts
{
    public class LabirintManager
    {
        private readonly CellManager cellManager;

        public LabirintManager(CellManager _cellManager)
        {
            cellManager = _cellManager;
        }

        public void CreateLabirint(Settings settings)
        {
            var cells = cellManager.cells;
            cellManager.Visited(0);

            var unvisitedCellsCount = cells.Length - 1;

            var currentCellIndex = 0;

            while (unvisitedCellsCount > 0)
            {
                if ((cells[currentCellIndex] & CellManager.maskAllNeighbours) != 0)
                {
                    int randomUnvisitedNeghbourIndex = currentCellIndex + cellManager.GetRandomUnvisitedNeghbourRelativePosition(cells[currentCellIndex]);
                    cellManager.RemoveWall(currentCellIndex, randomUnvisitedNeghbourIndex);

                    cells[randomUnvisitedNeghbourIndex] &= ~CellManager.maskCameFromLC;
                    cells[randomUnvisitedNeghbourIndex] |= currentCellIndex << 9; //TODO

                    currentCellIndex = randomUnvisitedNeghbourIndex;
                    cellManager.Visited(currentCellIndex);
                    unvisitedCellsCount--;
                }
                else //if (path.Count > 0)
                {
                    currentCellIndex = (int)(cells[currentCellIndex] & CellManager.maskCameFromLC) >> 9; //TODO
                }
            }

            ChangeDifficulty(settings.labirintDifficulty);
        }

        private void ChangeDifficulty(int difficulty)
        {
            for (int i = 0; i < cellManager.cells.Length; i++)
            {
                if (cellManager.CellWallsCount(cellManager.cells[i]) > difficulty)
                {
                    var randomWall = 0;
                    while (randomWall == 0)
                    {
                        var rd = Random.Range(0, 4);
                        //берем маску с самым правым битом из масок стен и смещаем на случайное число
                        randomWall = (int)(cellManager.cells[i] & (CellManager.maskWallTop << rd));
                    }
                    
                    cellManager.RemoveWall(i, i + cellManager.GetNeighbourRelativePosition(randomWall << 4));
                }
            }
        }

    }
}