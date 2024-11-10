using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;

    public CardSO selectedCard;

    private void Start() 
    {
        playerList = playerListObject.list;
        playerList.Clear();
        playerList.Add(player1);
    }
}
