using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;

    // Setup the game over screen
    public void Setup()
    {
        gameObject.SetActive(true);

    }

    // Restart the game by loading the play scene
    public void RestartButton()
    {
        SceneManager.LoadScene("PlayScene");
    }
    // Quit to main menu by loading the start menu scene
    public void QuitButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
