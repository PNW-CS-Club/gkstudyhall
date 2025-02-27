using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject pauseButtonContainer;
    [SerializeField] GameObject optionsContainer;
    
    [SerializeField] GameObject soundPanel;
    [SerializeField] GameObject videoPanel;
    
    [SerializeField] GameObject soundControls;
    [SerializeField] GameObject videoControls;

    GameObject currControls;

    void Awake()
    {
        pauseBackground.SetActive(false);
        GameIsPaused = false;

        pauseButtonContainer.SetActive(true);
        optionsContainer.SetActive(false);
        
        soundControls.SetActive(false);
        videoControls.SetActive(false);
        currControls = soundControls;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }
    
    public void Resume()
    {
        pauseBackground.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    void Pause()
    {
        pauseBackground.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenOptions()
    {
        optionsContainer.SetActive(true);
        pauseButtonContainer.SetActive(false);
        SelectSoundTab();
    }

    public void CloseOptions()
    {
        optionsContainer.SetActive(false);
        pauseButtonContainer.SetActive(true);
    }

    void SelectTab(GameObject panel, GameObject controls)
    {
        // disable previous controls, enable the ones in the selected tab
        currControls.SetActive(false);
        currControls = controls;
        currControls.SetActive(true);
        
        // this puts the selected panel in front of the rest of them
        panel.transform.SetAsLastSibling();
    }
    public void SelectSoundTab() => SelectTab(soundPanel, soundControls);
    public void SelectVideoTab() => SelectTab(videoPanel, videoControls);
    
    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene("CharSelectScene");
    }
    
    public void QuitGame()
    {
        // save any game data here
        
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
