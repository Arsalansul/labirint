using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Main : MonoBehaviour
    {
        public Button gameButton;
        public Button quitButton;

        public CellManager cellManager;

        private Settings settings;

        void Start()
        {
            gameButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(Quit);

            settings = new Settings();
            settings.labirintSize = 15;

            cellManager = new CellManager(settings.labirintSize);

            //for (int i = 0; i < cellManager.cells.Length; i++)
            //{
            //    Debug.Log("cell " + cellManager.cells[i]);
            //}
        }

        private void Quit()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            Game game = new Game(cellManager, settings);
        }
    }
}
