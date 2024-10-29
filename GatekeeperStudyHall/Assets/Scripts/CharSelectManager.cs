using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> playerList; // refers to list in playerListObject
    
    [SerializeField] PlayerSO player1;


    // Awake is called as soon as the object is created
    private void Start() 
    {
        playerList = playerListObject.list;
        playerList.Clear();
        playerList.Add(player1);
    }

}
