using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    // https://medium.com/@nicholas.w.swift/easy-a-star-pathfinding-7e6689c7f7b2
    public class PathFinder
    {
        private CellManager cellManager;
        public PathFinder(CellManager _cellManager)
        {
            cellManager = _cellManager;
        }

        private void FindPath(int currentCellIndex, int endCellIndex)
        {
            cellManager.cells[currentCellIndex] |= CellManager.maskOpenListPF; //add current cell to open list
            while (OpenListCount() > 0)
            {
                currentCellIndex = GetCellIndexWithLeastF(); // let the currentNode equal the node with the least f value

                cellManager.cells[currentCellIndex] &= ~CellManager.maskOpenListPF; //remove from open list
                cellManager.cells[currentCellIndex] |= CellManager.maskCloseListPF; //add to close list

                if (currentCellIndex == endCellIndex) //found the end
                {
                    break;
                }

                for (var i = 0; i < 4; i++)
                {
                    //find child
                    var maskWall = CellManager.maskWallTop << i;
                    if ((cellManager.cells[currentCellIndex] & maskWall) != 0)
                        continue;

                    var childIndex = currentCellIndex + cellManager.GetNeighbourRelativePosition(maskWall << 4);
                    
                    // Child is on the closedList
                    if ((cellManager.cells[childIndex] & CellManager.maskCloseListPF) != 0)
                        continue;

                    cellManager.cells[childIndex] |= (ulong)currentCellIndex << CellManager.CameFromFirstBitPF; //запоминаем откуда пришли
                    
                    // Create the f, g, and h values
                    //child.g = currentNode.g + distance between child and current
                    //child.h = distance from child to end
                    //child.f = child.g + child.h;

                    var g = ((cellManager.cells[currentCellIndex] & CellManager.maskGPF) >> CellManager.GFirstBitPF) + 1;
                    if ((cellManager.cells[childIndex] & CellManager.maskOpenListPF) != 0 &&
                        g > ((cellManager.cells[childIndex] & CellManager.maskGPF) >> CellManager.GFirstBitPF))
                        continue;

                    cellManager.cells[childIndex] |= g << CellManager.GFirstBitPF;

                    var h = cellManager.Distance(childIndex, endCellIndex);
                    cellManager.cells[childIndex] |= (ulong)h << CellManager.HFirstBitPF;

                    // Add the child to the openList
                    cellManager.cells[childIndex] |= CellManager.maskOpenListPF;
                }
            }
        }

        public int GiveCellIndexToMove(int startCellIndex, int endCellIndex)
        {
            FindPath(startCellIndex, endCellIndex);

            //проходим путь от конца к началу
            var currentCellIndex = endCellIndex;
            while (currentCellIndex != startCellIndex)
            {
                var previusIndex = (int)((cellManager.cells[currentCellIndex] & CellManager.maskCameFromPF) >> CellManager.CameFromFirstBitPF);
                cellManager.cells[previusIndex] |= (ulong)currentCellIndex << CellManager.MoveToIndexFirstBitPF; //записываем путь

                currentCellIndex = previusIndex;
            }

            var result = (int) ((cellManager.cells[startCellIndex] & CellManager.maskMoveTo) >>
                                CellManager.MoveToIndexFirstBitPF);

            ClearCells();

            return result;
        }

        private void ClearCells()
        {
            for (int i = 0; i < cellManager.cells.Length; i++)
            {
                cellManager.cells[i] &= ~CellManager.maskAllPF;
            }
        }

        private int OpenListCount()
        {
            var result = 0;
            for (int i = 0; i < cellManager.cells.Length; i++)
            {
                result += (int)((cellManager.cells[i] & CellManager.maskOpenListPF) >> CellManager.OpenListFirstBitPF);
            }
            return result;
        }

        private int GetCellIndexWithLeastF()
        {
            var result = -1;
            ulong f_Old = 300;
            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                if ((cellManager.cells[i] & CellManager.maskOpenListPF) == 0) continue;
                var f = ((cellManager.cells[i] & CellManager.maskGPF) >> CellManager.GFirstBitPF) + ((cellManager.cells[i] & CellManager.maskHPF) >> CellManager.HFirstBitPF);

                if (f >= f_Old) continue;
                result = i;
                f_Old = f;
            }
            return result;
        }
    }
}
