using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField]
    private GameOverScreen gameOverScreen;

    public void Awake()
    {
        instance = this;

        // Get game over screen
        if (gameOverScreen == null)
        {
            gameOverScreen = GetComponent<GameOverScreen>();
        }
        if (gameOverScreen == null)
        {
            gameOverScreen = FindObjectOfType<GameOverScreen>();
        }

        gameOverScreen.gameObject.SetActive(false);
    }
    
    public void GameOver()
    {

        // Let ScoreManager finalize the score/time before showing the screen
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.OnGameOver();
        }
        gameOverScreen.gameObject.SetActive(true);
    }
}
