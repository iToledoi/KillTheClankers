using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference restart;

    private void OnEnable()
    {
        if (restart != null)
            restart.action.performed += RestartGame;
    }

    private void OnDisable()
    {
        if (restart != null)
            restart.action.performed -= RestartGame;
    }

    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
