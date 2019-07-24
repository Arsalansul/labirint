using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Main : MonoBehaviour
    {
        public Button gameButton;
        public Button quitButton;
        public Dropdown levelsDropdown;
        
        private Settings settings;

        private GameObject canvasGameObject;

        private Game game;


        void Start()
        {
            gameButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(Quit);
            levelsDropdown.options.Clear();
            for (var i = 0; i < 50; i++)
            {
                levelsDropdown.options.Add(new Dropdown.OptionData("level " + i));
            }

            levelsDropdown.value = 0;

            settings = new Settings();
            settings.labirintSize = 15;
            settings.labirintDifficulty = 1;
            settings.playerStartPosition = new Vector2(settings.labirintSize / 2, settings.labirintSize / 2);

            canvasGameObject = GameObject.Find("Canvas");

            SceneManager.LoadScene("Game", LoadSceneMode.Additive);
        }

        void Update()
        {
            if (settings.GameOver)
            {
                GameOver();
            }
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            SetSettingsValues(levelsDropdown.value);
            game = new Game(settings);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
            game.LoadSceneObjects();
            canvasGameObject.SetActive(false);
        }

        private void GameOver()
        {
            game.unitManager.DestroyUnits();
            canvasGameObject.SetActive(true);
            game.wallsCreator.DestoryWalls();
            game = null;
            settings.GameOver = false;
        }

        private void SetSettingsValues(int level)
        {
            settings.enemyCount = level % 20 + 3;
            settings.coinCount = level % 20 + 3;
            settings.enemySpeed = 0.9f;
            settings.playerSpeed = 1;
        }
    }
}
