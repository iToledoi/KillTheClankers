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
        //freeze time
        Time.timeScale = 0f;
        //set SoundMenuCanvas active
        GameObject.Find("SoundMenuCanvas").SetActive(true);
        //set StartMenuCanvas inactive
        gameObject.SetActive(false);
        


    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
