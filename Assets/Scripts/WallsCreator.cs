using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class WallsCreator : MonoBehaviour
{
    public GameObject WallGameObject;

    private GameObject walls;

    public void CreateWalls(CellManager cellManager, Settings settings)
    {
        var labirintSize = settings.labirintSize;
        walls = new GameObject("Walls");
        Vector3 position = new Vector3(0.5f, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);
        Vector3 deltaInRow = new Vector3(1, 0, 0);
        Vector3 deltaInColumn = new Vector3(-labirintSize, 0, 1);
        var cellDeltaPosition = new Vector3(0, 0, 0.5f);

        Vector3 exitDir = Vector3.back;

        var maskWall = CellManager.maskWallBottom;
        for (int v = 0; v < 2; v++) //горизонталь + вертикаль
        {
            for (int i = 0; i < labirintSize + 1; i++) // кол-во рядов
            {
                for (int j = 0; j < labirintSize; j++) //стены в одном ряде
                {
                    if (i == labirintSize && j ==0) //у последней строки/столбце меняем параметры, чтоб не вылезти за пределы массива
                    {
                        cellDeltaPosition = -cellDeltaPosition;
                        maskWall = maskWall >> 2;
                        exitDir = -exitDir;
                    }

                    var currentCellIndex = cellManager.GetCellIndexByPosition(position + cellDeltaPosition);
                    if ((cellManager.cells[currentCellIndex] & maskWall) != 0 )
                    {
                        var instance = Instantiate(WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                        if (cellManager.CheckExit(currentCellIndex, exitDir))
                        {
                            instance.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }
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

            exitDir = Vector3.left;
        }
    }

    public void DestoryWalls()
    {
        Destroy(walls);
    }
}
