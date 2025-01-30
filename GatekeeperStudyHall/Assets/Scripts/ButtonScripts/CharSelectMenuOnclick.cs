using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> players;
    public CardSO randomCard;
    public CardSO clearCard;
    
    
    [SerializeField] List<CardSO> cardList;

    void Start() 
    {
        players = playerListObject.list;   
    }

    public void StartGame()
    {
        foreach (var v in players)
        {
            if(v.card == clearCard)
                return;
        }

        //reset players
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GameReset();
            players[i].ResetEffects();
        }
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");

        foreach (var c in players)
        {
            if( c.card == randomCard ){
                int r = UnityEngine.Random.Range(0, cardList.Count);
                c.card = cardList[r];
            }
        }
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
