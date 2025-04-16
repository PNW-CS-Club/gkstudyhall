using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{

    public GameObject EndStatDisplay;
    List<GameObject> displayList;
    [SerializeField] PlayerListSO playerListSO;
    List<PlayerSO> playerList; // refers to list in playerListSO
    
    Vector3 offset; // the offset of the bottom three players

    // Start is called before the first frame update
    void Start()
    {
        playerList = playerListSO.list;
        displayList = new();
        Globals.sessionMatchesPlayed++;

        Debug.Log($"{Globals.winningPlayer} won the game!");      

        //set starting offset to top-left of screen
        offset = new Vector3(0f, 0f, 0f); // TODO: Find a good value for this

        // move the winning player to the front of the list
        playerList.Remove(Globals.winningPlayer);
        playerList.Insert(0, Globals.winningPlayer);

        //create all of the stat displays we need
        for(int i = 0; i < playerList.Count; i++){
            GameObject newStatDisplay = Instantiate(EndStatDisplay,transform);
            displayList.Add(newStatDisplay);
        }

        for(int i = 0; i < displayList.Count;i++){
            Transform displayTransform = displayList[i].transform;
            CardDisplay cardDisplay = displayTransform.GetChild(0).GetComponent<CardDisplay>();
            displayTransform.localPosition = offset;
            
            cardDisplay.ChangeCardData(playerList[i].card); 
            cardDisplay.player = playerList[i];
            offset.x += 350;
        }

    }

    void Awake(){
        

        // initialize the cards and displays
        /*
        for(int i = 0; i < playerList.Count; i++){
            GameObject newCard = Instantiate(cardPrefab, transform);
            cardObjectList.Add(newCard);

            // look at CardQueue.cs for example
            // TODO: make a statDisplay prefab

        }
        */
        /*
        // set the displays
        for(int i = 0; i < playerList.Count; i++){
            Transform cardTransform = cardObjectList[i].transform;
            CardDisplay cardDisplay = cardTransform.GetComponent<CardDisplay>();
            cardTransform.localPosition = offset;
            cardDisplay.ChangeCardData(playerList[i].card);
            cardDisplay.player = playerList[i];

            offset.x += 300;
        }
        */
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
