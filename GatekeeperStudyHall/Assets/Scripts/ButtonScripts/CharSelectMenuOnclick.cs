using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    [SerializeField] PlayerSO p4;
    List<PlayerSO> players;
    public CardSO randomCard;
    public CardSO clearCard;

    [SerializeField] GameObject Card3;
    [SerializeField] GameObject Card4;
    
    
    [SerializeField] List<CardSO> cardList;

    void Start() 
    {
        if(Globals.sessionMatchesPlayed == 0) 
            Globals.playerList = playerListObject.list;

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

    public void AddPlayer(PlayerSO p3)
    {
        //This function will be for the AddPlayerButton
        //When the button is clicked, the player should be added to the list of players
        if(players.Count < 4) {
            if(players.Count < 3) {
                players.Add(p3);
                Card3.SetActive(true); 
            }
            else {
                players.Add(p4);
                Card4.SetActive(true); 
            }
        }

    }

    public void AddBot()
    {
        //This function will be for the AddBotButton
        //When the button is clicked, an instance of the Bot class should be made and added to the list of players
    
    }

    public void RemovePlayer(PlayerSO p3)
    {
        //This function will be for the RemovePLayerButton on both Bots and Players
        //When the button is clicked, remove the Player/Bot from the list of players and make the card available again
        if(players.Count > 2) {
            if(players.Count > 3) {
                players.Remove(p4);
                Card4.SetActive(false); 
            }
            else {
                players.Remove(p3);
                Card3.SetActive(false); 
            }    
        }
    }
}
