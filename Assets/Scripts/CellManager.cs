using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class CellManager
    {
        public long[] cells;

        // 1-4 bits - walls
        // 5-8 - unvisited neighbours (for CreateLabirint)
        // 9 - visited  (for CreateLabirint)
        // 10-17 - came from cell index (for CreateLabirint)
        // 18-25 - came from cell index (for PathFinder)
        public const long maskWallTop = 1;
        public const long maskWallRight = 1 << 1;
        public const long maskWallBottom = 1 << 2;
        public const long maskWallLeft = 1 << 3;
                     
        public const long maskAllWalls = (1 << 4) - 1;
                     
        public const long maskNeighbourTop = 1 << 4;
        public const long maskNeighbourRight = 1 << 5;
        public const long maskNeighbourBottom = 1 << 6;
        public const long maskNeighbourLeft = 1 << 7;
                     
        public const long maskVisited = 1 << 8;
                    
        public const long maskCameFromLC = ((1 << 8) - 1) << 9; //for labirint creator
                   
        public const long maskCameFromPF = ((1 << 8) - 1) << 17; //for labirint creator
              
        public const long maskAllNeighbours = ((1 << 4) - 1) << 4;

        private readonly int labirintSize;

        public CellManager(int _labirintSize)
        {
            labirintSize = _labirintSize;
            cells = new long[labirintSize * labirintSize];
            DefaultCell();
        }

        public void DefaultCell()
        {
            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] |= maskAllWalls; //set walls

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

        public int GetNeighbourRelativePosition(long mask)
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
            if (neighbourIndex < 0 || neighbourIndex >= labirintSize * labirintSize) return;
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

        public int GetRandomUnvisitedNeghbourRelativePosition(long cell)
        {
            long neighbourPositionMask = 0; 
            if ((cell & maskAllNeighbours) != 0)
            {
                while (neighbourPositionMask == 0)
                {
                    var rd = Random.Range(0, 4);
                    //берем маску с самым правым битом из масок соседей и смещаем на случайное число
                    neighbourPositionMask = cell & (maskNeighbourTop << rd);
                }
                return GetNeighbourRelativePosition(neighbourPositionMask);
            }
            return 0;
        }

        public long GetCellByPosition(Vector3 position)
        {
            var x = (int) position.x;
            var y = (int) position.z;

            return cells[x + y * labirintSize];
        }

        public int CellWallsCount(long cell)
        {
            var result = 0;
            for (var i = 0; i < 4; i++)
            {
                result += ((int)cell & (1 << i)) >> i;
            }

            return result;
        }

        public int Distance(int cellIndex_1, int cellIndex_2) //минимальное расстояние без диагоналей
        {
            var result = 0;
            result += Mathf.Abs(cellIndex_1 % labirintSize - cellIndex_2 % labirintSize);
            result += Mathf.Abs(cellIndex_1 / labirintSize - cellIndex_2 / labirintSize);
            return result;
        }
    }
}
