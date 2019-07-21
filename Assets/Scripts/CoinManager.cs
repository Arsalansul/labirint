using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private PathFinder pathFinder = new PathFinder();
    private CellManager cellManager = CellManager.Instance;
    private readonly int pathMin = 5;
    private readonly Vector3 defaultVector = new Vector3(-100,-100,-100);
    private List<GameObject> coins;

    public void CoinsSpawn(int count)
    {
        coins = new List<GameObject>();
        var coinsParrent = new GameObject("Coins");
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = CoinSpawnPosition();
            if (spawnPosition == defaultVector) continue;
            var instance = Instantiate(Settings.Instance.coinSettings.CoinGameObject, spawnPosition, Quaternion.identity, coinsParrent.transform);
            coins.Add(instance);
        }
    }

    private Vector3 CoinSpawnPosition()
    {
        var labirintSize = Settings.Instance.gameSettings.labirintSize;

        Vector3 result = new Vector3();
        var pathMinSize = 0;
        var i = 0;
        while (pathMinSize < pathMin && i < labirintSize * labirintSize)
        {
            i++;
            pathMinSize = pathMin;
            var randomCell = cellManager.cells[Random.Range(0, labirintSize), Random.Range(0, labirintSize)];
            foreach (var coin in coins)
            {
                var path = pathFinder.GetPath(randomCell, cellManager.GetCellByTransform(coin.transform.position));
                if (path.Count < pathMinSize)
                {
                    pathMinSize = path.Count;
                }
            }
            result = cellManager.GetTransformByCell(randomCell);
        }

        if (i > 100)
        {
            Debug.LogError("Can't find spawn point for coin");
            return defaultVector;
        }
        
        return result;
    }
}
