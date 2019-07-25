using UnityEngine;

namespace Assets.Scripts
{
    public class UnitManager : MonoBehaviour
    {
        private GameObject enemyParent;
        private GameObject coinParent;

        public GameObject player;

        private GameObject Units;

        public void InstantiateUnits(Settings settings, CellManager cellManager)
        {
            Units = new GameObject("Units");

            player = Instantiate(Resources.Load("Prefabs/Player"),Units.transform) as GameObject;

            var playerUnit = player.GetComponent<Unit>();
            playerUnit.Pos = settings.playerStartPosition;
            playerUnit.moveController = 1;
            playerUnit.cellManager = cellManager;
            playerUnit.settings = settings;
            playerUnit.speed = settings.playerSpeed;

            enemyParent = new GameObject("enemies");
            enemyParent.transform.SetParent(Units.transform);
            for (var i = 0; i < settings.enemyCount; i++)
            {
                var enemy = Instantiate(Resources.Load("Prefabs/Enemy"), enemyParent.transform) as GameObject;

                var enemyUnit = enemy.GetComponent<Unit>();
                var enemyPos = new Vector2(Random.Range(0, settings.labirintSize), Random.Range(0, settings.labirintSize));
                enemyUnit.settings = settings;
                enemyUnit.Pos = enemyPos;
                enemyUnit.moveController = 2;
                enemyUnit.cellManager = cellManager;
                enemyUnit.target = player.transform;
                enemyUnit.speed = settings.enemySpeed;
            }

            coinParent = new GameObject("Coins");
            coinParent.transform.SetParent(Units.transform);

            for (int i = 0; i < settings.coinCount; i++)
            {
                GenerateCoinPosition(4, cellManager);
            }

            for (var i = 0; i < cellManager.cells.Length; i++)
            {
                if ((cellManager.cells[i] & CellManager.maskCoin) == 0)
                    continue;

                var coin = Instantiate(Resources.Load("Prefabs/Coin"), coinParent.transform) as GameObject;

                var coinUnit = coin.GetComponent<Unit>();
                coin.transform.position = cellManager.GetPositionByCellIndex(i);
                coinUnit.moveController = 0;
                coinUnit.cellManager = cellManager;
            }
        }

        public void DestroyUnits()
        {
            Destroy(Units);
        }

        private void GenerateCoinPosition(int distance, CellManager cellManager)
        {
            if (cellManager.CountCoinPositions() == 0)
            {
                Debug.LogError("Don't have free coin positions");
                return;
            }

            var cellIndex = Random.Range(0, cellManager.cells.Length);
            while ((cellManager.cells[cellIndex] & CellManager.maskCoinNegativePoint) != 0)
            {
                cellIndex = Random.Range(0, cellManager.cells.Length);
            }
            
            cellManager.SetCoinInCell(cellIndex);
            cellManager.CoinNegativePoint(cellIndex);
            SetCoinNegativePoints(cellIndex, distance, cellManager);

            cellManager.ClearCellsAfterPF();
        }
        
        private void SetCoinNegativePoints (int cellIndex, int distance, CellManager cellManager)
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

                    cellManager.CoinNegativePoint(childIndex);

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
    }
}
