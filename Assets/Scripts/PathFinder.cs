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
            while (cellManager.OpenListCount() > 0)
            {
                loop++;
                currentCellIndex = cellManager.GetCellIndexWithLeastF(); // let the currentNode equal the node with the least f value

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
                    
                    // Create the f, g, and h values
                    //child.g = currentNode.g + distance between child and current
                    //child.h = distance from child to end
                    //child.f = child.g + child.h;

                    var g = cellManager.GetG(currentCellIndex) + 1;

                    if ((cellManager.cells[childIndex] & CellManager.maskOpenListPF) != 0 && g > cellManager.GetG(childIndex))
                        continue;

                    cellManager.RememberG(childIndex, g);

                    var h = cellManager.Distance(childIndex, endCellIndex);
                    cellManager.RememberH(childIndex, (ulong) h);

                    cellManager.RememberCameFromIndexPF(childIndex, currentCellIndex); //запоминаем откуда пришли

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

        public void FindCellsInDistance(int cellIndex, int distance)  //в бит маску G записывается длина пути, этим и будем пользоваться
        {
            cellManager.AddToOpenList(cellIndex);

            var loop = 0;
            while (cellManager.OpenListCount() > 0)
            {
                loop++;

                cellIndex = cellManager.GetCellIndexFromOpenList();

                cellManager.RemoveFromOpenList(cellIndex);
                cellManager.AddToCloseList(cellIndex);

                for (var i = 0; i < 4; i++)
                {
                    //find child
                    var maskWall = CellManager.maskWallTop << i;
                    if ((cellManager.cells[cellIndex] & maskWall) != 0)
                        continue;

                    var childIndex = cellIndex + cellManager.GetNeighbourRelativePosition(maskWall << 4);

                    // Child is on the closedList
                    if ((cellManager.cells[childIndex] & CellManager.maskCloseListPF) != 0)
                        continue;

                    //cellManager.CoinNegativePoint(childIndex);

                    var g = cellManager.GetG(cellIndex) + 1;

                    if ((cellManager.cells[childIndex] & CellManager.maskOpenListPF) != 0 && g > cellManager.GetG(childIndex))
                        continue;

                    cellManager.RememberG(childIndex, g);

                    // Add the child to the openList
                    if (g < (ulong)distance)
                        cellManager.AddToOpenList(childIndex);
                }

                if (loop > 300)
                {
                    Debug.LogError("open list does not end");
                    break;
                }
            }
        }

        public int GiveCellIndexToMove(int startCellIndex, int endCellIndex)
        {
            FindPath(startCellIndex, endCellIndex);

            //выстраиваем путь от конца к началу
            var currentCellIndex = endCellIndex;
            var loop = 0;
            while (currentCellIndex != startCellIndex)
            {
                loop++;
                
                var nextIndex = cellManager.GetCameFromIndexPF(currentCellIndex);

                cellManager.RememberMoveToIndex(nextIndex, currentCellIndex); //записываем путь

                if (loop > 1000)
                {
                    Debug.LogError(" h " + cellManager.GetH(nextIndex));
                    cellManager.ClearCellsAfterPF();
                    return startCellIndex;
                }
                currentCellIndex = nextIndex;
            }

            var result = cellManager.GetMoveToIndexPF(startCellIndex);

            cellManager.ClearCellsAfterPF();

            return result;
        }

        public int GetRandomCellIndexInDistance(int cellIndex, int distance)
        {
            FindCellsInDistance(cellIndex, distance);

            for (int i = 0; i < cellManager.cells.Length; i++)
            {
                var randomIndex = Random.Range(0, cellManager.cells.Length);
                if (cellManager.GetG(randomIndex) != 0)
                {
                    cellManager.ClearCellsAfterPF();
                    return randomIndex;
                }
            }
            cellManager.ClearCellsAfterPF();
            return cellIndex;
        }
    }
}
