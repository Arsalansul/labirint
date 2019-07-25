using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : MonoBehaviour
    {
        public int moveController; //0 - none, 1- player, 2 - AI
        public CellManager cellManager;
        public Transform target;

        public float speed;
        private bool detectTarget;

        private PathFinder pathFinder;
        private Vector3 nextPosition;
        private Vector3 startPosition;

        public Settings settings;

        private Vector2 changeAxis;
        public Vector2 Pos
        {
            get
            {
                var pos = transform.position;

                return new Vector2(pos.x, pos.z);
            }
            set => transform.position = new Vector3(value.x + 0.5f, 0, value.y + 0.5f);
        }

        void Start()
        {
            pathFinder = new PathFinder(cellManager);
            nextPosition = transform.position;
            startPosition = transform.position;
            changeAxis = new Vector2(1,0);
        }

        void Update()
        {
            if (moveController > 0)
            {
                GetNextPosition();
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);
            }
        }

        private void GetNextPosition()
        {
            if ((transform.position - nextPosition).magnitude > 0.05f) return;

            if (moveController == 1 && InputKeyPressed())
            {
                var nextCellIndex = cellManager.GetCellIndexByPosition(transform.position + GetInputVector().normalized);
                
                if (!cellManager.CheckWall(cellManager.GetCellIndexByPosition(transform.position), nextCellIndex))
                    nextPosition = cellManager.GetPositionByCellIndex(nextCellIndex);
            }
            else if (moveController == 2 && transform.position != target.position)
            {
                Radar();
                var currentCell = cellManager.GetCellIndexByPosition(transform.position);
                int cellIndexToMove;
                if (detectTarget)
                {
                    cellIndexToMove = pathFinder.GiveCellIndexToMove(currentCell, cellManager.GetCellIndexByPosition(target.position));
                }
                else
                {
                    var randomCellInDistance = pathFinder.GetRandomCellIndexInDistance(currentCell, 3);
                    
                    cellIndexToMove = pathFinder.GiveCellIndexToMove(currentCell, randomCellInDistance);
                }
                nextPosition = cellManager.GetPositionByCellIndex(cellIndexToMove);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "enemy" && gameObject.tag == "Player")
                settings.GameOver = true;

            if (other.tag == "Player")
                Destroy(gameObject);
        }

        private Vector3 GetInputVector()
        {
            var inputVector = new Vector3(0, 0, 0);
            if (Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") > 0.05f && Input.GetAxis("Vertical") * Input.GetAxis("Vertical") > 0.05f)
            {
                inputVector.x = Input.GetAxis("Horizontal") * changeAxis.x;
                inputVector.z = Input.GetAxis("Vertical") * changeAxis.y;

                var temp = changeAxis.x;
                changeAxis.x = changeAxis.y;
                changeAxis.y = temp;
            }
            else if (Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") > 0.05f)
            {
                inputVector.x = Input.GetAxis("Horizontal");
                inputVector.z = 0;
            }
            else if (Input.GetAxis("Vertical") * Input.GetAxis("Vertical") > 0.05f)
            {
                inputVector.x = 0;
                inputVector.z = Input.GetAxis("Vertical");
            }

            return inputVector;
        }

        private bool InputKeyPressed()
        {
            return Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") +
                   Input.GetAxis("Vertical") * Input.GetAxis("Vertical") > 0.05f;
        }

        private void Radar()
        {
            var distance = (target.position - transform.position).magnitude;
            if (distance < settings.enemyDetectTargetDistance)
                detectTarget = true;

            if (distance > settings.enemyLostTargetDistance)
                detectTarget = false;
        }
    }
}
