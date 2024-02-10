using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleOnclick : MonoBehaviour
{
    public void StartGame() {
        // loads the main game scene asynchronously to prevent stutter
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
    }

    public void QuitGame() {
        // stops the exe application
        Application.Quit();

        // stops the in-editor application
        UnityEditor.EditorApplication.isPlaying = false; 
    }
}
