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

    public Button generateLab;

    void Start()
    {
        cellManager = new CellManager();
        labirintManager = new LabirintManager();
        labirintManager.GenerateLab(cellManager, Settings.Instance.gameSettings.labirintSize);

        var playerSpawnPosition = cellManager.GetTransformByCell(cellManager.cells[
            (int) Settings.Instance.gameSettings.labirintSize / 2,
            (int) Settings.Instance.gameSettings.labirintSize / 2]);
        player = Instantiate(Settings.Instance.playerSettings.PlayerGameObject, playerSpawnPosition,
            Quaternion.identity);

        enemy = Instantiate(Settings.Instance.enemySettings.EnemyrGameObject, playerSpawnPosition + new Vector3(3, 0, 3),
            Quaternion.identity);

        mainCamera = Camera.main;
        mainCamera.transform.position = player.transform.position + mainCamera.GetComponent<MainCamera>().offset;
        mainCamera.transform.eulerAngles = new Vector3(90, 0, 0);
        mainCamera.GetComponent<MainCamera>().target = player.transform;

#if DEBUG
        generateLab.gameObject.SetActive(true);
        generateLab.onClick.AddListener(() => labirintManager.GenerateLab(cellManager, Settings.Instance.gameSettings.labirintSize));
#endif
    }

    // Update is called once per frame
    void Update()
    {
        enemy.GetComponent<EnemyController>().Move(cellManager, player.transform, Settings.Instance.gameSettings.labirintSize);

#if DEBUG
        if (Input.GetKeyDown(KeyCode.P))
        {
            enemy.AddComponent<LineRenderer>();
            enemy.GetComponent<LineRenderer>().widthMultiplier = 0.3f;
            enemy.GetComponent<EnemyController>().DrawPath(cellManager);
        }
#endif
    }

    
}
