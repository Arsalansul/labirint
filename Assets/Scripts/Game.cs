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

        private WallsCreator wallsCreator;

        public Game(Settings _settings)
        {
            settings = _settings;
            cellManager = new CellManager(settings.labirintSize);
            labirintManager = new LabirintManager(cellManager);
        }

        private void LoadSceneObjects()
        {
            wallsCreator = new WallsCreator();
            wallsCreator.WallGameObject = Resources.Load<GameObject>("Prefabs/Wall");
            labirintManager.CreateLabirint(settings);
            wallsCreator.CreateWalls(cellManager, settings);
        }

        public IEnumerator LoadingScene()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);

            yield return new WaitForFixedUpdate();
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
            LoadSceneObjects();
        }

    }
}
