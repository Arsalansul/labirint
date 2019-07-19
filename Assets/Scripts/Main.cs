using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Button gameButton;

    public Button quitButton;

    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        gameButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(Quit);
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
