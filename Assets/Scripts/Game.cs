using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private CellManager cellManager;
    private LabirintCreator labirintCreator;

    public Button generateLab;

    void Start()
    {
        cellManager = new CellManager();
        labirintCreator = new LabirintCreator();
        labirintCreator.GenerateLab(cellManager, Settings.Instance.gameSettings.labirintSize);


        generateLab.onClick.AddListener(() => labirintCreator.GenerateLab(cellManager, Settings.Instance.gameSettings.labirintSize));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
