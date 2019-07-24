using UnityEngine;

namespace Assets.Scripts
{
    public class Unit :MonoBehaviour
    {
        public int moveController; //0 - none, 1- player, 2 - AI
        public CellManager cellManager;
        public Transform target;

        private PathFinder pathFinder;
        private Vector3 nexpPosition;

        public Vector2 Pos
        {
            get
            {
                var pos = transform.position;

                return new Vector2(pos.x, pos.z);
            }
            set
            {
               transform.position = new Vector3(value.x + 0.5f, 0, value.y + 0.5f);
            }
        }

        void Start()
        {
            pathFinder = new PathFinder(cellManager);
            nexpPosition = transform.position;
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            if (moveController == 2 && transform.position == nexpPosition && transform.position != target.position)
            {
                var cellIndexToMove = pathFinder.GiveCellIndexToMove(cellManager.GetCellIndexByPosition(transform.position), cellManager.GetCellIndexByPosition(target.position));
                nexpPosition = cellManager.GetPositionByCellIndex(cellIndexToMove);
                
            }
            transform.position = Vector3.MoveTowards(transform.position, nexpPosition, Time.deltaTime);
        }
    }
}
