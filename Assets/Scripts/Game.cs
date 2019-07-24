using System.Collections;
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
        private Camera mainCamera;
        private MainCamera mainCameraComponent;

        public Game(Settings _settings)
        {
            settings = _settings;
            cellManager = new CellManager(settings.labirintSize);
            labirintManager = new LabirintManager(cellManager);
            wallsCreator = new WallsCreator();
            unitManager = new UnitManager();
            mainCamera = Camera.main;
            mainCameraComponent = mainCamera.GetComponent<MainCamera>();
            mainCameraComponent.settings = settings;
            wallsCreator.WallGameObject = Resources.Load<GameObject>("Prefabs/Wall");
        }

        public void LoadSceneObjects()
        {
            labirintManager.CreateLabirint(settings);
            wallsCreator.CreateWalls(cellManager, settings);
            
            unitManager.InstantiateUnits(settings, cellManager);
            
            mainCameraComponent.target = unitManager.player.transform;
        }
    }
}
