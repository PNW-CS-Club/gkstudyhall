using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenuOnClick : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    [SerializeField] PlayerListSO backupListObject;
    List<PlayerSO> players;
    public CardSO randomCard;
    public CardSO clearCard;

    [SerializeField] PlayerSO p2;   
    [SerializeField] PlayerSO p3;
    [SerializeField] PlayerSO p4;
    [SerializeField] GameObject Card2;
    [SerializeField] GameObject Card3;
    [SerializeField] GameObject Card4;
    
    
    [SerializeField] List<CardSO> cardList;

    //in here we set the list of players before starting the game
    void Start()  
    {
        if(Globals.sessionMatchesPlayed == 0) //if it is the first game make the globalplayerlist a shadow copy of the playerlistobject
            Globals.playerList = playerListObject.list;
        players = playerListObject.list;  
        
    }

    public void StartGame()
    {
        foreach (var v in players)
        {
            if(v.card == clearCard || players.Count < 2)
                return;
        }

        //reset players
        foreach (var player in players)
        {
            player.GameReset();
            player.ResetEffects();
        }
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");

        foreach (var c in players)
        {
            if( c.card == randomCard){
                int r = UnityEngine.Random.Range(0, cardList.Count);
                c.card = cardList[r];
            }
        }
    }
    public void StartMenu()
    {
        AsyncOperation _ = SceneManager.LoadSceneAsync("StartScene");
    }

    ///This function is for the AddPlayerButton.
    ///When the button is clicked, the player should be added to the list of players.
    public void AddPlayer()
    {
        switch (players.Count)
        {
            case 1:
                players.Add(p2);
                Card2.SetActive(true);
                p2.isBot = false;
                p2.card = clearCard;
                return;
            case 2:
                players.Add(p3);
                Card3.SetActive(true);
                p3.isBot = false;
                p3.card = clearCard;
                return;
            case 3:
                players.Add(p4);
                Card4.SetActive(true);
                p4.isBot = false;
                p4.card = clearCard;
                return;
            case >= 4:
                return;
        }
        
    }

    ///This function will be for the AddBotButton.
    ///When the button is clicked, an instance of the Bot class should be made and added to the list of players.
    public void AddBot()
    {
        switch (players.Count)
        {
            case 1:
                players.Add(p2);
                Card2.SetActive(true);
                p2.isBot = true;
                p2.card = randomCard;
                return;
            case 2:
                players.Add(p3);
                Card3.SetActive(true);
                p3.isBot = true;
                p3.card = randomCard;
                return;
            case 3:
                players.Add(p4);
                Card4.SetActive(true);
                p4.isBot = true;
                p4.card = randomCard;
                return;
            case >= 4:
                return;
        }
    }

    ///This function is for the RemovePlayerButton on both Bots and Players.
    ///When the button is clicked, remove the Player/Bot from the list of players and make the card available again.
    public void RemovePlayer()
    {
        switch (players.Count)
        {
            case <= 1:
                return;
            case 2:
                players.Remove(p2);
                Card2.SetActive(false);
                return;
            case 3:
                players.Remove(p3);
                Card3.SetActive(false);
                return;
            case >= 4:
                players.Remove(p4);
                Card4.SetActive(false);
                return;
        }
    }
}
