using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Main : MonoBehaviour
    {
        public Button gameButton;
        public Button quitButton;
        
        private Settings settings;

        private GameObject canvasGameObject;

        private Game game;


        void Start()
        {
            gameButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(Quit);

            settings = new Settings();
            settings.labirintSize = 15;
            settings.labirintDifficulty = 2;

            canvasGameObject = GameObject.Find("Canvas");
            
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            game = new Game(settings);
            StartCoroutine(game.LoadingScene());
            canvasGameObject.SetActive(false);
        }
    }
}
