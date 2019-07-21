using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private Camera mainCamera;

    private CellManager cellManager;
    private LabirintManager labirintManager;

    private GameObject player;
    private GameObject enemy;
    private EnemyController enemyController;
    private CoinManager coinManager;

    void Start()
    {
        cellManager = CellManager.Instance;
        labirintManager = new LabirintManager();
        labirintManager.GenerateLab();

        coinManager = new CoinManager();

        var playerSpawnPosition = cellManager.GetTransformByCell(cellManager.cells[
            (int) Settings.Instance.gameSettings.labirintSize / 2,
            (int) Settings.Instance.gameSettings.labirintSize / 2]);
        player = Instantiate(Settings.Instance.playerSettings.PlayerGameObject, playerSpawnPosition, Quaternion.identity);

        var enemySpawnPosition = cellManager.GetTransformByCell(cellManager.cells[
            Random.Range(0, Settings.Instance.gameSettings.labirintSize), 
            Random.Range(0, Settings.Instance.gameSettings.labirintSize)]);
        enemy = Instantiate(Settings.Instance.enemySettings.EnemyrGameObject, enemySpawnPosition, Quaternion.identity);
        enemyController = enemy.GetComponent<EnemyController>();

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
    }

    
}
