using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;
    [SerializeField] PlayerSO player2;
    [SerializeField] PlayerSO player3;
    [SerializeField] PlayerSO player4;

    [SerializeField] GameObject Card2;
    [SerializeField] GameObject Card3;
    [SerializeField] GameObject Card4;

    public CardSO selectedCard;

    public CardSO clearCard;

    
    //Every time we check if this is a new game or not
    //if is a new game we clear everything and set the game for 2 players
    //if is not a new game we are setting everything whit the last game information
    private void Start() 
    {
        if(Globals.sessionMatchesPlayed == 0) {
            playerList = playerListObject.list;
            playerList.Clear();
            playerList.Add(player1);
            player1.card = clearCard;
            player2.card = clearCard;
            player3.card = clearCard; 
            player4.card = clearCard;  
            Card2.SetActive(false);
            Card3.SetActive(false); 
            Card4.SetActive(false);   
        }
        else {
            playerListObject.list = Globals.playerList; //if it's not the first game we are making a shadow copy of the globalplayerlist
            playerList = playerListObject.list;
            Card2.SetActive(playerList.Count >= 2);
            Card3.SetActive(playerList.Count >= 3);
            Card4.SetActive(playerList.Count == 4);     
        }
    }
}
