using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuOnclick : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName)
    {
        // loads the scene asynchronously to prevent stutter
        AsyncOperation _ = SceneManager.LoadSceneAsync(sceneName);
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
