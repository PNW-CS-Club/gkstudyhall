using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuOnclick : MonoBehaviour
{
    public void StartGame() {
        // loads the main game scene asynchronously to prevent stutter
        AsyncOperation _ = SceneManager.LoadSceneAsync("CharSelectScene");
    }

    public void QuitGame() {
        // stops the exe application
        Application.Quit();

        #if UNITY_EDITOR
            // stops the in-editor application
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
