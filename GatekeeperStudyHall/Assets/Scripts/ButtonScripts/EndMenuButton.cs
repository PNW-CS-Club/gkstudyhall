using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuButton : MonoBehaviour
{
    public void MainMenu()
    {
        AsyncOperation _= SceneManager.LoadSceneAsync("StartScene");
    }
}
