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

            while (unvisitedCellsCount > 0)
            {
                if ((cells[currentCellIndex] & CellManager.maskAllNeighbours) != 0)
                {
                    int randomUnvisitedNeghbourIndex = currentCellIndex + cellManager.GetRandomUnvisitedNeghbourIndex(cells[currentCellIndex]);
                    cellManager.RemoveWall(currentCellIndex, randomUnvisitedNeghbourIndex);

                    cells[randomUnvisitedNeghbourIndex] &= ~CellManager.maskCameFromLC;
                    cells[randomUnvisitedNeghbourIndex] |= (currentCellIndex << 9); //TODO

                    currentCellIndex = randomUnvisitedNeghbourIndex;
                    cellManager.Visited(currentCellIndex);
                    unvisitedCellsCount--;
                }
                else //if (path.Count > 0)
                {
                    currentCellIndex = (cells[currentCellIndex] & CellManager.maskCameFromLC) >> 9; //TODO
                }
            }
        }

    }
}