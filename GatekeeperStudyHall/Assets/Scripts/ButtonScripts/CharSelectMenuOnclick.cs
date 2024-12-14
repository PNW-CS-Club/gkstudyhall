using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> players;

    void Start() 
    {
        players = playerListObject.list;
        //reset players
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GameReset();
            players[i].ResetEffects();
        }
    }

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
        if (!players.Contains(p)) 
        {
            players.Add(p);
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
        if (players.Contains(p)) 
        {
            players.Remove(p);
        }
    }
}
