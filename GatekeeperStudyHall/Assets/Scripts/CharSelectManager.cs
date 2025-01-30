using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;
    [SerializeField] PlayerSO player2;

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
        }
        else {
            playerList = playerListObject.list;
        }
    }
}
