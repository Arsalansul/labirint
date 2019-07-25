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

        public void LabirintCreator(Settings settings)
        {
            for (int i = 0; i < settings.labirintDifficulty; i++)
            {
                Labirint();
            }
        }

        private void Labirint()
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
                    currentCellIndex = (int)(cells[currentCellIndex] & CellManager.maskCameFromLC) >>
                                       CellManager.CameFromFirstBitLC;
                }
            }
        }

    }
}