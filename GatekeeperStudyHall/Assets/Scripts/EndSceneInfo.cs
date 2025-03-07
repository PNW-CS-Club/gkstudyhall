using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] PlayerListSO playerListSO;
    List<PlayerSO> playerList; // refers to list in playerListSO
    List<GameObject> cardObjectList; // instances of cardDisplay
    List<GameObject> statDisplayList; // instances of statDisplay

    Vector3 offset; // the offset of the bottom three players

    // Start is called before the first frame update
    void Start()
    {
        Globals.sessionMatchesPlayed++;

        Debug.Log($"{Globals.winningPlayer} won the game!");
                
    }

    void Awake(){
        playerList = playerListSO.list;

        // move the winning player to the front of the list
        playerList.Remove(Globals.winningPlayer);
        playerList.Insert(0, Globals.winningPlayer);
           
        // create a list of card displays for each player
        cardObjectList = new(playerList.Count); // same length as playerList
        statDisplayList = new(playerList.Count); 

        //set starting offset to top-left of screen
        offset = new Vector3(100f, 50f, 0f); // TODO
       
        // initialize the cards and displays
        for(int i = 0; i < playerList.Count; i++){
            GameObject newCard = Instantiate(cardPrefab, transform);
            cardObjectList.Add(newCard);

            // look at CardQueue.cs for example
            // TODO: make a statDisplay prefab

        }

        // set the displays
        for(int i = 0; i < playerList.Count; i++){
            Transform cardTransform = cardObjectList[i].transform;
            CardDisplay cardDisplay = cardTransform.GetComponent<CardDisplay>();
            cardTransform.localPosition = offset;
            cardDisplay.ChangeCardData(playerList[i].card);
            cardDisplay.player = playerList[i];

            offset.x += 300;
        }
        
        /*
        statDisplay1.GetComponent<TMP_Text>().text = 
            $"Total Damage Dealt to Other Players: {Globals.winningPlayer.totalDamageToOtherPlayers}\n" +
            $"Total Damage Dealt to Gates: {Globals.winningPlayer.totalDamageToGates}\n" +
            $"Total Damage Dealt to Gatekeeper: {Globals.winningPlayer.totalDamageToGatekeeper}\n" +
            $"Total Amount Healed: {Globals.winningPlayer.totalAmountHealed}\n" +
            $"Total Damage Taken: {Globals.winningPlayer.totalDamageTaken}\n" +
            $"Total Stockade Collected: {Globals.winningPlayer.totalStockade}\n" +
            $"Battles Started: {Globals.winningPlayer.battlesStarted}\n";
        */
    }

    // Update is called once per frame
    void Update() {}
}
