﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class WallsCreator : MonoBehaviour
{
    public GameObject WallGameObject;

    public void CreateWalls(CellManager cellManager, Settings settings)
    {
        var labirintSize = settings.labirintSize;
        GameObject walls = new GameObject("Walls");
        Vector3 position = new Vector3(0.5f, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);
        Vector3 deltaInRow = new Vector3(1, 0, 0);
        Vector3 deltaInColumn = new Vector3(-labirintSize, 0, 1);
        var cellDeltaPosition = new Vector3(0, 0, 0.5f);

        Vector3 exit = new Vector3(Random.Range(0, 2), Random.Range(0,2), Random.Range(0, labirintSize));
        if (exit.y > 0)
            exit.y = labirintSize;

        var maskWall = CellManager.maskWallBottom;
        for (int v = 0; v < 2; v++) //горизонталь + вертикаль
        {
            for (int i = 0; i < labirintSize + 1; i++) // кол-во рядов
            {
                for (int j = 0; j < labirintSize; j++) //стены в одном ряде
                {
                    if (i == 0 || i == labirintSize) //стены границу
                    {
                        if (v != exit.x || i!=exit.y || j !=exit.z)
                            Instantiate(WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                    }
                    else if ((cellManager.GetCellByPosition(position + cellDeltaPosition) & maskWall) != 0 )
                    {
                        Instantiate(WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                    }
                    position += deltaInRow;
                }
                position += deltaInColumn;
            }

            position = new Vector3(0, 0, 0.5f);
            rotation = new Vector3(0, 90, 0);
            deltaInRow = new Vector3(0, 0, 1);
            deltaInColumn = new Vector3(1, 0, -labirintSize);
           
            maskWall = CellManager.maskWallLeft;
            cellDeltaPosition = new Vector3(0.5f, 0, 0);
        }
    }
}
