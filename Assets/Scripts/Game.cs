using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private Vector3 startPosition = new Vector3(0, 0, 0); //нижний левый угол

    private CellManager cellManager;
    private int labirintSize;

    public Button generateLab;

    void Start()
    {
        generateLab.onClick.AddListener(GenerateLab);
        labirintSize = Settings.Instance.gameSettings.labirintSize;

        cellManager = new CellManager();
        cellManager.CreateCells();
        CreateLabirint();
        CreateWalls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateWalls()
    {
        GameObject walls = new GameObject("Walls");
        Vector3 position = startPosition + new Vector3(0.5f,0,0);
        Vector3 rotation = new Vector3(0,0,0);
        Vector3 deltaInRow = new Vector3(1,0,0);
        Vector3 deltaInColumn = new Vector3(-Settings.Instance.gameSettings.labirintSize, 0, 1);
        var cellDeltaPosition = new Vector3(0, 0, 0.5f);

        var wall = Cell.Wall.Down;
        for (int v = 0; v < 2; v++)
        {
            for (int i = 0; i < Settings.Instance.gameSettings.labirintSize + 1; i++)
            {
                for (int j = 0; j < Settings.Instance.gameSettings.labirintSize; j++)
                {
                    if (i==0 || i== 15)
                        Instantiate(Settings.Instance.wallSettings.WallGameObject, position, Quaternion.Euler(rotation),walls.transform);
                    else if (cellManager.GetCellByTransform(position + cellDeltaPosition).Walls.Contains(wall))
                    {
                        Instantiate(Settings.Instance.wallSettings.WallGameObject, position, Quaternion.Euler(rotation), walls.transform);
                    }
                    position += deltaInRow;
                }
                position += deltaInColumn;
            }

            position = startPosition + new Vector3(0, 0, 0.5f);
            rotation = new Vector3(0, 90, 0);
            deltaInRow = new Vector3(0, 0, 1);
            deltaInColumn = new Vector3(1, 0, -Settings.Instance.gameSettings.labirintSize);
            wall = Cell.Wall.Left;
            cellDeltaPosition = new Vector3(0.5f, 0, 0);
        }
    }

   
    private void CreateLabirint()
    {
        List<Cell> unvisitedCells = new List<Cell>();
        for (int i = 0; i < labirintSize; i++)
        {
            for (int j = 0; j < labirintSize; j++)
            {
                unvisitedCells.Add(cellManager.cells[i, j]);
            }
        }

        Cell currentCell = cellManager.cells[0, 0]; //TODO random start cell
        unvisitedCells.Remove(currentCell);

        Stack<Cell> path = new Stack<Cell>();
        while (unvisitedCells.Count>0)
        {
            var randomUnvisitedNeighbour = cellManager.GetRandomNeighbourCellFromList(currentCell, unvisitedCells);
            if (randomUnvisitedNeighbour != null)
            {
                path.Push(currentCell);
                currentCell.CellCanMoveTo.Add(randomUnvisitedNeighbour);
                cellManager.RemoveWall(currentCell,randomUnvisitedNeighbour);
                currentCell = randomUnvisitedNeighbour;
                unvisitedCells.Remove(currentCell);
            }
            else if (path.Count > 0)
            {
                currentCell = path.Pop();
            }
            else
            {
                currentCell = unvisitedCells[Random.Range(0, unvisitedCells.Count)];
                unvisitedCells.Remove(currentCell);
            }
        }
    }

    public void GenerateLab()
    {
        Destroy(GameObject.Find("Walls"));
        cellManager.CreateCells();
        CreateLabirint();
        CreateWalls();
    }
}
