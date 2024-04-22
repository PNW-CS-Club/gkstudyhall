using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    public void StartGame()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
    }
    public void StartMenu()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("StartScene");
    }

    public void AddPlayer(PlayerSO p)
    {
        //This function will be for the AddPlayerButton
        //When the button is clicked, the player should be added to the list of players
        if(MainManager.Instance != null && !MainManager.Instance.PlayerList.Contains(p)){
            MainManager.Instance.PlayerList.Add(p);
        }
        

    }

    public void AddBot()
    {
        //This function will be for the AddBotButton
        //When the button is clicked, an instance of the Bot class should be made and added to the list of players
    
    }

    public void RemovePlayer(PlayerSO p)
    {
        //This function will be for the RemovePLayerButton on both Bots and Players
        //When the button is clicked, remove the Player/Bot from the list of players and make the card available again
        if(MainManager.Instance != null && MainManager.Instance.PlayerList.Contains(p)){
            MainManager.Instance.PlayerList.Remove(p);
        }
    }
}
