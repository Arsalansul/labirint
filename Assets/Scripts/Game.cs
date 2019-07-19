using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private CellManager cellManager;
    private LabirintCreator labirintCreator;
    private int labirintSize;

    public Button generateLab;

    void Start()
    {
        labirintSize = Settings.Instance.gameSettings.labirintSize;

        cellManager = new CellManager();
        labirintCreator = new LabirintCreator();
        labirintCreator.GenerateLab(cellManager, labirintSize);


        generateLab.onClick.AddListener(() => labirintCreator.GenerateLab(cellManager, labirintSize));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
