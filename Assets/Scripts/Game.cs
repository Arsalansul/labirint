﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Game
    {
        private CellManager cellManager;
        private LabirintManager labirintManager;
        private Settings settings;

        public WallsCreator wallsCreator;
        public UnitManager unitManager;

        public Game(Settings _settings)
        {
            settings = _settings;
            cellManager = new CellManager(settings.labirintSize);
            labirintManager = new LabirintManager(cellManager);
            wallsCreator = new WallsCreator();
            unitManager = new UnitManager();
        }

        public void LoadSceneObjects()
        {
            wallsCreator.WallGameObject = Resources.Load<GameObject>("Prefabs/Wall");
            
            labirintManager.CreateLabirint(settings);
            wallsCreator.CreateWalls(cellManager, settings);
            
            unitManager.InstantiateUnits(settings, cellManager);
        }

    }
}
