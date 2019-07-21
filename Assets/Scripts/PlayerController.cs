using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 nextPosition;
    private CellManager cellManager = CellManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || transform.position != nextPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * Settings.Instance.enemySettings.speed);
            if (transform.position != nextPosition) return;
            GoToNextPoint();
        }
    }

    private void GoToNextPoint()
    {
        var currentCell = cellManager.GetCellByTransform(transform.position);
        var passableNeighbours = cellManager.GetPassableNeighbours(currentCell);

        var moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Debug.Log(moveVector);
        var nextCell = cellManager.cells[currentCell.x + (int) moveVector.x, currentCell.y + (int) moveVector.z];
        if (passableNeighbours.Contains(nextCell))
            nextPosition = cellManager.GetTransformByCell(nextCell);
    }
}
