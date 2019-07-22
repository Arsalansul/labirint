using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class CellManager
    {
        public int[] cells;

        // 1-4 bits - walls
        // 5-8 - unvisited neighbours (for CreateLabirint)
        // 9 - visited  (for CreateLabirint)
        public const int maskWallTop = 1;
        public const int maskWallRight = 1 << 1;
        public const int maskWallBottom = 1 << 2;
        public const int maskWallLeft = 1 << 3;

        public const int maskNeighbourTop = 1 << 4;
        public const int maskNeighbourRight = 1 << 5;
        public const int maskNeighbourBottom = 1 << 6;
        public const int maskNeighbourLeft = 1 << 7;

        public const int maskVisited = 1 << 8;

        public const int maskCameFromLC = ((1 << 8) - 1) << 9; //for labirint creator

        public const int maskAllNeighbours = ((1 << 4) - 1) << 4;

        private readonly int labirintSize;

        public CellManager(int _labirintSize)
        {
            labirintSize = _labirintSize;
            cells = new int[labirintSize * labirintSize];
            DefaultCell();
        }

        public void DefaultCell()
        {
            var walls = maskWallTop | maskWallRight | maskWallBottom | maskWallLeft;
            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] |= walls; //set walls

                //set unvisited neighbours
                if (i < cells.Length - labirintSize)
                {
                    cells[i] |= maskNeighbourTop;
                }

                if ((i + 1) % labirintSize != 0)
                {
                    cells[i] |= maskNeighbourRight;
                }

                if (i >= labirintSize)
                {
                    cells[i] |= maskNeighbourBottom;
                }

                if (i % labirintSize != 0)
                {
                    cells[i] |= maskNeighbourLeft;
                }
            }
        }

        public void Visited(int cellIndex)
        {
            cells[cellIndex] |= maskVisited;
            if (cellIndex < cells.Length - labirintSize)
                cells[cellIndex + labirintSize] &= ~maskNeighbourBottom; //говорим верхнему соседу, что соседа снизу уже посетили

            if((cellIndex +1)%labirintSize !=0)
                cells[cellIndex + 1] &= ~maskNeighbourLeft; //говорим соседу справа, что соседа слева уже посетили

            if(cellIndex >= labirintSize)
                cells[cellIndex - labirintSize] &= ~maskNeighbourTop; //говорим соседу снизу, что соседа сверху уже посетили

            if(cellIndex % labirintSize != 0)
                cells[cellIndex - 1] &= ~maskNeighbourRight; //говорим соседу слева, что соседа справа уже посетили
        }

        public int GetNeighbourIndex(int mask)
        {
            switch (mask)
            {
                case maskNeighbourTop:
                    return labirintSize;
                case maskNeighbourRight:
                    return 1;
                case maskNeighbourBottom:
                    return -15;
                case maskNeighbourLeft:
                    return -1;
                default:
                    return 0;
            }
        }

        public void RemoveWall(int cellIndex, int neighbourIndex)
        {
            var indexDif = cellIndex - neighbourIndex;
            if (indexDif == -labirintSize)
            {
                cells[cellIndex] &= ~maskWallTop; //remove top wall
                cells[neighbourIndex] &= ~maskWallBottom; //remove bottom wall
            }
            else if (indexDif == -1)
            {
                cells[cellIndex] &= ~maskWallRight; //remove right wall
                cells[neighbourIndex] &= ~maskWallLeft; //remove left wall
            }
            else if (indexDif == labirintSize)
            {
                cells[cellIndex] &= ~maskWallBottom; //remove bottom wall
                cells[neighbourIndex] &= ~maskWallTop; //remove top wall
            }
            else if (indexDif == 1)
            {
                cells[cellIndex] &= ~maskWallLeft; //remove left wall
                cells[neighbourIndex] &= ~maskWallRight; //remove right wall
            }
        }

        public int GetRandomUnvisitedNeghbourIndex(int cell)
        {
            var neighbourPositionMask = 0; 

            if ((cell & ((1 << 4) - 1)<<4) != 0)
            {
                while (neighbourPositionMask == 0)
                {
                    var rd = Random.Range(0, 4);
                    if ((cell & (maskNeighbourTop << rd)) != 0)
                        neighbourPositionMask = maskNeighbourTop << rd;
                }

                return GetNeighbourIndex(neighbourPositionMask);
            }
            Debug.LogError("dotn't have unvisited neubours. Cell " + cell);
            return 0;
        }

        public int GetCellByPosition(Vector3 position)
        {
            var x = (int) position.x;
            var y = (int) position.z;

            return cells[x + y * labirintSize];
        }
    }
}
