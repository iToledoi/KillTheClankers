using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartMenuScreen : MonoBehaviour
{
    public void StartGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
    }

    public void SettingsButton()
    {
        // Implement settings menu later
        Debug.Log("Settings button clicked");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
