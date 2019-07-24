using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class CellManager
    {
        public ulong[] cells;

        // 1-4 bits - walls
        // 5-8 - unvisited neighbours (for CreateLabirint)
        // 9 - visited  (for CreateLabirint)
        // 10-17 - came from cell index (for CreateLabirint)

        // 18 - 25 - came from cell index (for PathFinder)
        // 26 - in open list (A Star терминология) (for PathFinder)
        // 27 - in close list (A Star терминология) (for PathFinder)
        // 28 - 35 - g value (A Star терминология) (for PathFinder)
        // 36 - 43 - h value (A Star терминология) (for PathFinder)
        // 44 - 51 - move to cell index (for PathFinder)

        public const ulong maskWallTop = 1;
        public const ulong maskWallRight = (ulong)1 << 1;
        public const ulong maskWallBottom = (ulong)1 << 2;
        public const ulong maskWallLeft = (ulong)1 << 3;
                     
        public const ulong maskAllWalls = ((ulong)1 << 4) - 1;
                     
        public const ulong maskNeighbourTop = (ulong)1 << 4;
        public const ulong maskNeighbourRight = (ulong)1 << 5;
        public const ulong maskNeighbourBottom = (ulong)1 << 6;
        public const ulong maskNeighbourLeft = (ulong)1 << 7;

        public const ulong maskAllNeighbours = (((ulong)1 << 4) - 1) << 4;

        public const ulong maskVisited = (ulong)1 << 8;
                    
        public const ulong maskCameFromLC = (((ulong)1 << 8) - 1) << 9; //for labirint creator
        public const int GetCameFromLC = 9; //for labirint creator

        public const ulong maskCameFromPF = (((ulong)1 << 8) - 1) << 17; //for path finder
        public const int CameFromFirstBitPF = 17; //for path finder
        public const ulong maskOpenListPF = (ulong)1 << 25; //for path finder
        public const int OpenListFirstBitPF = 25; //for path finder
        public const ulong maskCloseListPF = (ulong)1 << 26; //for path finder
        public const ulong maskGPF = (((ulong)1 << 8) - 1) << 27; //for path finder
        public const int GFirstBitPF = 27; //for path finder
        public const ulong maskHPF = (((ulong)1 << 8) - 1) << 35; //for path finder
        public const int HFirstBitPF = 35; //for path finder
        public const ulong maskMoveTo = (((ulong)1 << 8) - 1) << 43; //for path finder
        public const int MoveToIndexFirstBitPF = 43; //for path finder
        public const ulong maskAllPF = (((ulong)1 << 34) - 1) << 17; //for path finder


        private readonly int labirintSize;

        public CellManager(int _labirintSize)
        {
            labirintSize = _labirintSize;
            cells = new ulong[labirintSize * labirintSize];
            DefaultCell();
        }

        private void DefaultCell()
        {
            for (var i = 0; i < cells.Length; i++)
            {
                cells[i] |= maskAllWalls; //set walls

                SetUnvisitedNeighbours();
            }
        }

        public void SetUnvisitedNeighbours()
        {
            for (var i = 0; i < cells.Length; i++)
            {
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

        public int GetNeighbourRelativePosition(ulong mask)
        {
            switch (mask)
            {
                case maskNeighbourTop:
                    return labirintSize;
                case maskNeighbourRight:
                    return 1;
                case maskNeighbourBottom:
                    return -labirintSize;
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

        public int GetRandomUnvisitedNeghbourRelativePosition(ulong cell)
        {
            ulong neighbourPositionMask = 0; 
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

        public ulong GetCellByPosition(Vector3 position)
        {
            var x = (int) position.x;
            var y = (int) position.z;

            return cells[x + y * labirintSize];
        }

        public int GetCellIndexByPosition(Vector3 position)
        {
            var x = (int)position.x;
            var y = (int)position.z;

            return x + y * labirintSize;
        }

        public Vector3 GetPositionByCellIndex(int cellIndex)
        {
            var x = cellIndex % labirintSize + 0.5f;
            var y = 0;
            var z = (cellIndex / labirintSize) + 0.5f;

            return new Vector3(x, y, z);
        }

        public int CellWallsCount(ulong cell)
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
