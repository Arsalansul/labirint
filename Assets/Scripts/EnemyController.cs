using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private List<Cell> moveCells;
    private Vector3 nextPosition;
    PathFinder pathFinder = new PathFinder();

    private void GoToNextPoint(CellManager cellManager)
    {
        if (moveCells == null) return;
        nextPosition = cellManager.GetTransformByCell(moveCells[0]);  //отправляем на следующую клетку
        moveCells.RemoveAt(0); //удаляем клетку на которой стоим из массива
    }

    private void GetMoveCells(CellManager cellManager, Transform target, int labirintSize)
    {
        moveCells = pathFinder.GetPath(cellManager.GetCellByTransform(transform.position), cellManager.GetCellByTransform(target.position), cellManager, labirintSize);
    }

    public void Move(CellManager cellManager, Transform target, int labirintSize)
    {
        if (cellManager.GetCellByTransform(transform.position) ==
            cellManager.GetCellByTransform(target.position)) return;
        if (transform.position == nextPosition || moveCells==null)
        {
            GetMoveCells(cellManager, target, labirintSize);
            GoToNextPoint(cellManager);
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * Settings.Instance.enemySettings.speed);
    }

    public void DrawPath(CellManager cellManager)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = moveCells.Count + 1;

        lineRenderer.SetPosition(0, transform.position);
        var index = 0;
        foreach (var cell in moveCells)
        {
            index++;
            lineRenderer.SetPosition(index, cellManager.GetTransformByCell(cell));
        }
    }
}
