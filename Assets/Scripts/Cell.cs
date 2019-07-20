using System.Collections.Generic;

public class Cell
{
    public int x;
    public int y;

    public int g, h, f;

    public List<Cell> CellCanMoveTo = new List<Cell>();

    public List<Cell> UnvisitedNeighbour = new List<Cell>();

    public Cell CameFrom = null;

    public enum Wall
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public List<Wall> Walls;
}