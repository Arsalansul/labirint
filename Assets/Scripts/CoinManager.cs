using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private PathFinder pathFinder = new PathFinder();

    public void CoinsSpawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(Settings.Instance.coinSettings.CoinGameObject, CoinSpawnPosition(), Quaternion.identity);
        }
    }

    private Vector3 CoinSpawnPosition()
    {

        throw new System.NotImplementedException();
    }
}
