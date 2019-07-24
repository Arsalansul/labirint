﻿using System;
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
        private readonly CellManager cellManager;
        public PathFinder(CellManager _cellManager)
        {
            cellManager = _cellManager;
        }

        
        private void FindPath(int currentCellIndex, int endCellIndex)
        {
            cellManager.cells[currentCellIndex] |= CellManager.maskOpenListPF; //add current cell to open list
            var loop = 0;
            while (OpenListCount() > 0)
            {
                loop++;
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

                    var g = GetG(currentCellIndex) + 1;
                    if ((cellManager.cells[childIndex] & CellManager.maskOpenListPF) != 0 && g > GetG(childIndex))
                        continue;

                    cellManager.cells[childIndex] |= g << CellManager.GFirstBitPF;

                    var h = cellManager.Distance(childIndex, endCellIndex);
                    cellManager.cells[childIndex] |= (ulong)h << CellManager.HFirstBitPF;

                    // Add the child to the openList
                    cellManager.cells[childIndex] |= CellManager.maskOpenListPF;
                }

                if (loop > 300)
                {
                    Debug.LogError("path not found");
                    break;
                }
            }
        }

        public int GiveCellIndexToMove(int startCellIndex, int endCellIndex)
        {
            //ClearCells();
            FindPath(startCellIndex, endCellIndex);

            //проходим путь от конца к началу
            var currentCellIndex = endCellIndex;
            var loop = 0;
            while (currentCellIndex != startCellIndex)
            {
                loop++;
                
                var nextIndex = (int)((cellManager.cells[currentCellIndex] & CellManager.maskCameFromPF) >> CellManager.CameFromFirstBitPF);
                if (nextIndex > 224 || nextIndex < 0)
                {
                    Debug.LogError("out of range. next index " + nextIndex + " current index " + currentCellIndex + " current cell " + cellManager.cells[currentCellIndex]);
                }
                cellManager.cells[nextIndex] |= (ulong)currentCellIndex << CellManager.MoveToIndexFirstBitPF; //записываем путь

                if (loop > 1000)
                {
                    Debug.LogError(" h " + (cellManager.cells[nextIndex] & CellManager.maskHPF >> CellManager.HFirstBitPF));
                    //ulong neighbourIndex = 0;
                    //for (var i = 0; i < 4; i++)
                    //{
                    //    //find child
                    //    var maskWall = CellManager.maskWallTop << i;
                    //    if ((cellManager.cells[currentCellIndex] & maskWall) != 0)
                    //        continue;
                    //
                    //    neighbourIndex = currentCellIndex + (ulong)cellManager.GetNeighbourRelativePosition(maskWall << 4);
                    //}
                    //
                    ClearCells();
                    return startCellIndex;
                }
                currentCellIndex = nextIndex;
            }

            var result = (int) ((cellManager.cells[startCellIndex] & CellManager.maskMoveTo) >> CellManager.MoveToIndexFirstBitPF);

            ClearCells();

            return result;
        }

        private void ClearCells()
        {
            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                cellManager.cells[i] &= ~CellManager.maskAllPF;
            }
        }

        private int OpenListCount()
        {
            var result = 0;
            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                result += (int)((cellManager.cells[i] & CellManager.maskOpenListPF) >> CellManager.OpenListFirstBitPF);
            }
            return result;
        }

        private int GetCellIndexWithLeastF()
        {
            var result = -1;
            uint f_Old = 300;
            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                if ((cellManager.cells[i] & CellManager.maskOpenListPF) == 0) continue;
                //f = g + h
                var f =(uint)(((cellManager.cells[i] & CellManager.maskGPF) >> CellManager.GFirstBitPF) + ((cellManager.cells[i] & CellManager.maskHPF) >> CellManager.HFirstBitPF));

                if (f >= f_Old) continue;
                result = i;
                f_Old = f;
            }
            if (result == -1)
                Debug.LogError("f_Old " + f_Old);
            return result;
        }

        private ulong GetG(ulong cellIndex)
        {
            return ((cellManager.cells[cellIndex] & CellManager.maskGPF) >> CellManager.GFirstBitPF);
        }
    }
}
