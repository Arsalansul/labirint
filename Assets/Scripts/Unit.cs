using UnityEngine;

namespace Assets.Scripts
{
    public class Unit :MonoBehaviour
    {
        public int moveController; //0 - none, 1- player, 2 - AI
        public CellManager cellManager;

        private PathFinder pathFinder;

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
            Debug.Log(pathFinder.GiveCellIndexToMove(cellManager.GetCellIndexByPosition(transform.position), 1));
            //var moveToIndex = (cellManager.GetCellByPosition(transform.position) & CellManager.maskMoveToPF) >> 25;
            //Debug.Log(transform.position + " " + moveToIndex);
        }

        void Update()
        {
            //Move();
        }

        private void Move()
        {
            if (moveController == 2)
            {
                //pathFinder.GetPath(cellManager.GetCellIndexByPosition(transform.position), cellManager.GetCellIndexByPosition(Vector3.zero));
                //var moveToIndex = (cellManager.GetCellByPosition(transform.position) & CellManager.maskMoveToPF) >> 25;
                //transform.position = Vector3.MoveTowards(transform.position, cellManager.GetPositionByCellIndex((int)moveToIndex), Time.deltaTime);
            }
        }
    }
}
