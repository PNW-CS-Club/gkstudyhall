using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuOnclick : MonoBehaviour
{
    [SerializeField, Min(0.01f)] float loadDelay = 0.2f;

    IEnumerator LoadAfterDelayCoroutine() {
        yield return new WaitForSeconds(loadDelay);
        
        // loads the scene asynchronously to prevent stutter
        var _ = SceneManager.LoadSceneAsync("CharSelectScene");
        yield return null;
    }
    
    public void LoadAfterDelay() {
        StartCoroutine(LoadAfterDelayCoroutine());
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
