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

    [SerializeField] PlayerSO p3;
    [SerializeField] PlayerSO p4;
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
            if(v.card == clearCard)
                return;
        }

        //reset players
        foreach (var player in players)
        {
            player.GameReset();
            player.ResetEffects();
        }
        AsyncOperation _ = SceneManager.LoadSceneAsync("GkScene");
        // RESET CLOCK HERE - THIS BRANCH DOES NOT HAVE THE CLOCK YET

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

    ///This function is for the AddPlayerButton.
    ///When the button is clicked, the player should be added to the list of players.
    public void AddPlayer()
    {
        switch (players.Count)
        {
            case <= 2:
                players.Add(p3);
                Card3.SetActive(true);
                return;
            case 3:
                players.Add(p4);
                Card4.SetActive(true);
                return;
            case >= 4:
                return;
        }
    }

    ///This function will be for the AddBotButton.
    ///When the button is clicked, an instance of the Bot class should be made and added to the list of players.
    public void AddBot()
    {
    
    }

    ///This function is for the RemovePlayerButton on both Bots and Players.
    ///When the button is clicked, remove the Player/Bot from the list of players and make the card available again.
    public void RemovePlayer()
    {
        switch (players.Count)
        {
            case <= 2:
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
