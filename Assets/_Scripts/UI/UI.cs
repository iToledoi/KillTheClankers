using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference restart;
    
    // Subscribe to input events
    private void OnEnable()
    {
        if (restart != null)
            restart.action.performed += RestartGame;
    }
    // Unsubscribe from input events
    private void OnDisable()
    {
        if (restart != null)
            restart.action.performed -= RestartGame;
    }

    // Restart the current game scene
    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
