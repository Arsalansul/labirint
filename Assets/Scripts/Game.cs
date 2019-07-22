using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Game
    {
        private Camera mainCamera;

        private CellManager cellManager;
        private LabirintManager labirintManager;
        private Settings settings;

        private WallsCreator wallsCreator;

        //private GameObject player;
        //private GameObject enemy;
        //private EnemyController enemyController;
        //private CoinManager coinManager;

        public Game(CellManager _cellManager, Settings _settings)
        {
            cellManager = _cellManager;
            settings = _settings;
            labirintManager = new LabirintManager(cellManager);
            LoadScene();
        }

        public void LoadScene()
        {
            //SceneManager.LoadScene("Game", LoadSceneMode.Single);
            //GameObject go = new GameObject("New go");
            OnEnable();
            OnSceneLoaded(SceneManager.GetSceneByName("Game") , LoadSceneMode.Single);
        }

        // called first
        void OnEnable()
        {
            Debug.Log("OnEnable called");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);


            wallsCreator = new WallsCreator();
            wallsCreator.WallGameObject = Resources.Load<GameObject>("Prefabs/Wall");
            labirintManager.CreateLabirint();
            wallsCreator.CreateWalls(cellManager, settings);
        }

        // called third
        void Start()
        {
            Debug.Log("Start");
        }

        // called when the game is terminated
        void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /*void Start()
        {
            //labirintManager.GenerateLab();
            //
            //coinManager = new CoinManager();

            //var playerSpawnPosition = cellManager.GetTransformByCell(cellManager.cells[
            //    (int) Settings.Instance.gameSettings.labirintSize / 2,
            //    (int) Settings.Instance.gameSettings.labirintSize / 2]);
            //player = Instantiate(Settings.Instance.playerSettings.PlayerGameObject, playerSpawnPosition, Quaternion.identity);
            //
            //var enemySpawnPosition = cellManager.GetTransformByCell(cellManager.cells[
            //    Random.Range(0, Settings.Instance.gameSettings.labirintSize), 
            //    Random.Range(0, Settings.Instance.gameSettings.labirintSize)]);
            //enemy = Instantiate(Settings.Instance.enemySettings.EnemyrGameObject, enemySpawnPosition, Quaternion.identity);
            //enemyController = enemy.GetComponent<EnemyController>();

            coinManager.CoinsSpawn(2);

            mainCamera = Camera.main;
            mainCamera.transform.position = player.transform.position + mainCamera.GetComponent<MainCamera>().offset;
            mainCamera.transform.eulerAngles = new Vector3(90, 0, 0);
            mainCamera.GetComponent<MainCamera>().target = player.transform;
        }

        // Update is called once per frame
        void Update()
        {
            enemyController.Move(player.transform, 3, 5); //TODO сделать зависимость от уровня

#if DEBUG
            if (Input.GetKeyDown(KeyCode.P))
            {
                enemy.AddComponent<LineRenderer>();
                enemy.GetComponent<LineRenderer>().widthMultiplier = 0.3f;
                enemy.GetComponent<EnemyController>().DrawPath();
            }

            if (Input.GetKeyDown(KeyCode.L))
                labirintManager.GenerateLab();

            if (Input.GetKeyDown(KeyCode.C))
                coinManager.CoinsSpawn(5);
#endif
        }*/




    }
}
