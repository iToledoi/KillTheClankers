using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas mainMenuCanvas;    // The main menu canvas
    [SerializeField] private Canvas settingsCanvas;    // The settings menu canvas
    
    private void Awake()
    {
        // Try to find references if not set
        if (mainMenuCanvas == null)
            mainMenuCanvas = GetComponent<Canvas>();
        if (settingsCanvas == null)
            settingsCanvas = GameObject.Find("SoundMenuCanvas")?.GetComponent<Canvas>();

        // Ensure proper initial state
        if (mainMenuCanvas) mainMenuCanvas.enabled = true;
        if (settingsCanvas) settingsCanvas.gameObject.SetActive(false);
    }
    public void StartGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
    }

    public void SettingsButton()
    {
        if (settingsCanvas == null)
        {
            Debug.LogWarning("Settings canvas not assigned!");
            return;
        }

        // Show settings menu
        settingsCanvas.gameObject.SetActive(true);
        
        // Hide main menu
        if (mainMenuCanvas != null)
            mainMenuCanvas.enabled = false;
    }

    public void BackButton()
    {
        // Hide settings menu
        if (settingsCanvas != null)
            settingsCanvas.gameObject.SetActive(false);

        // Show main menu
        if (mainMenuCanvas != null)
            mainMenuCanvas.enabled = true;
    }

    // Quit the application
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
