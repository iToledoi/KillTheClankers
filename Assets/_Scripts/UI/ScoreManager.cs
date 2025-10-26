using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    // UI Text Mesh Pro elements to display score and high score
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public TextMeshProUGUI finalScoreText;

    //UI text for Game Over Screen
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI bestTimeText;

    int score = 0;
    int highScore = 0;

    float time = 0f;
    float bestTime = 0f;

    // Controls whether the timer is running (player alive)
    private bool timerRunning = true;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
        finalScoreText.text = score.ToString();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highScore.ToString();


    }
    
    void Update()
    {
        // Update time only while the player is alive / timer is running
        if (!timerRunning) return;

        // Update time
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // Update best time if current time exceeds it
        if (time > bestTime)
        {
            bestTime = time;
            bestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

    }

    // Stop the timer and update final UI values. Call this when the player dies / game over.
    public void OnGameOver()
    {
        timerRunning = false;

        // Set final score and time on the Game Over screen
        if (finalScoreText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time % 60);
            string timeStr = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Method to add points to the current score based on number of enemies defeated
    public void AddScore()
    {
        score += 1;
        scoreText.text = score.ToString();
        finalScoreText.text = score.ToString();

        // Update high score if current score exceeds it
        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }


    //following code allows you to easily reset the PlayerPrefs HighScore
    [Tooltip("Check this box to reset the HighScore in PlayerPrefs")]
    public bool resetHighScoreNow = false;

    // Draw Gizmos in editor to reset high score and time if checkbox is ticked
    void OnDrawGizmos()
    {
        if (resetHighScoreNow)
        {
            resetHighScoreNow = false;
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetFloat("BestTime", 0);
            Debug.LogWarning("PlayerPrefs HighScore and BestTime reset to 0.");
        }
    }
}
