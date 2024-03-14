using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
    }

    // Update is called once per frame
    public void StartMenu()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("StartScene");
    }
}
