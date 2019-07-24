using UnityEngine;

namespace Assets.Scripts
{
    public class UnitManager : MonoBehaviour
    {
        public void InstantiateUnits(Settings settings, CellManager cellManager)
        {
            var player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;

            var playerUnit = player.GetComponent<Unit>();
            playerUnit.Pos = settings.playerStartPosition;
            playerUnit.moveController = 1;
            playerUnit.cellManager = cellManager;

            GameObject enemyParent = new GameObject("enemies");
            for (var i = 0; i < settings.enemyCount; i++)
            {
                var enemy = Instantiate(Resources.Load("Prefabs/Enemy"), enemyParent.transform) as GameObject;

                var enemyUnit = enemy.GetComponent<Unit>();
                var enemyPos = new Vector2(Random.Range(0, settings.labirintSize), Random.Range(0, settings.labirintSize));
                enemyUnit.Pos = enemyPos;
                enemyUnit.moveController = 2;
                enemyUnit.cellManager = cellManager;
                enemyUnit.target = player.transform;
            }

            GameObject coinParent = new GameObject("Coins");
            for (var i = 0; i < settings.coinCount; i++)
            {
                var coin = Instantiate(Resources.Load("Prefabs/Coin"), coinParent.transform) as GameObject;

                var coinUnit = coin.GetComponent<Unit>();
                var coinPos = new Vector2(Random.Range(0, settings.labirintSize), Random.Range(0, settings.labirintSize));
                coinUnit.Pos = coinPos;
                coinUnit.moveController = 0;
                coinUnit.cellManager = cellManager;
            }
        }
    }
}
