using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;
    [SerializeField] PlayerSO player2;

    [SerializeField] GameObject Card3;
    [SerializeField] GameObject Card4;

    public CardSO selectedCard;

    public CardSO clearCard;

    

    private void Start() 
    {
        if(Globals.sessionMatchesPlayed == 0) {
            playerList = playerListObject.list;
            playerList.Clear();
            playerList.Add(player1);
            playerList.Add(player2);
            player1.card = clearCard;   
            player2.card = clearCard;
            Card3.SetActive(false); 
            Card4.SetActive(false);   
        }
        else {
            playerListObject.list = Globals.playerList;
            playerList = playerListObject.list;
            Card3.SetActive(playerList.Count >= 3);
            Card4.SetActive(playerList.Count == 4);     
        }
    }
}
