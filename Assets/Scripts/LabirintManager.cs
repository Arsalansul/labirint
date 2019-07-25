using UnityEngine;

namespace Assets.Scripts
{
    //https://habr.com/ru/post/262345/
    public class LabirintManager
    {
        private readonly CellManager cellManager;

        public LabirintManager(CellManager _cellManager)
        {
            cellManager = _cellManager;
        }

        public void CreateLabirint(Settings settings)
        {
            for (int i = 0; i < settings.labirintDifficulty; i++)
            {
                var cells = cellManager.cells;
                cellManager.SetUnvisitedNeighbours();

                cellManager.Visited(0);

                var unvisitedCellsCount = cells.Length - 1;

                var currentCellIndex = 0;

                while (unvisitedCellsCount > 0)
                {
                    if ((cells[currentCellIndex] & CellManager.maskAllNeighbours) != 0)
                    {
                        int randomUnvisitedNeghbourIndex =
                            currentCellIndex +
                            cellManager.GetRandomUnvisitedNeghbourRelativePosition(cells[currentCellIndex]);
                        cellManager.RemoveWall(currentCellIndex, randomUnvisitedNeghbourIndex);

                        cells[randomUnvisitedNeghbourIndex] &= ~CellManager.maskCameFromLC;
                        cellManager.RememberCameFromIndexLC(randomUnvisitedNeghbourIndex, currentCellIndex);
                        
                        currentCellIndex = randomUnvisitedNeghbourIndex;
                        cellManager.Visited(currentCellIndex);
                        unvisitedCellsCount--;
                    }
                    else //if (path.Count > 0)
                    {
                        currentCellIndex = (int) (cells[currentCellIndex] & CellManager.maskCameFromLC) >>
                                           CellManager.CameFromFirstBitLC;
                    }
                }
            }
        }

        private void ChangeDifficulty(Settings settings)
        {
            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                if (cellManager.CellWallsCount(cellManager.cells[i]) > settings.labirintDifficulty)
                {
                    ulong randomWall = 0;
                    while (randomWall == 0)
                    {
                        var rd = Random.Range(0, 4);
                        //берем маску с самым правым битом из масок стен и смещаем на случайное число
                        randomWall = (cellManager.cells[i] & (CellManager.maskWallTop << rd));
                    }
                    
                    //проверка на крайние стены
                    if(i < settings.labirintSize && randomWall == CellManager.maskWallBottom)
                        continue;
                    if (i % settings.labirintSize == 0 && randomWall == CellManager.maskWallLeft)
                        continue;
                    if (i >= settings.labirintSize * (settings.labirintSize - 1)  && randomWall == CellManager.maskWallTop)
                        continue;
                    if (i % settings.labirintSize == settings.labirintSize - 1 && randomWall == CellManager.maskWallRight)
                        continue;

                    cellManager.RemoveWall(i, i + cellManager.GetNeighbourRelativePosition(randomWall << 4));
                }
            }
        }

    }
}