using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{

    public GameObject EndStatDisplay;
    public GameObject WinnerTextDisplay;
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

        Debug.Log($"{Globals.winningPlayer} Wins!");      

        // Set starting offset to middle-left of screen
        offset = new Vector3(-Screen.width/3f,  -250f, 0f); // (player display dimensions are (300x500) and pivot is lower-left corner (0,0) ) 

        // Move the winning player to the front of the list
        playerList.Remove(Globals.winningPlayer);
        playerList.Insert(0, Globals.winningPlayer);

        // Announce the winner of the game
        GameObject newTextObject = Instantiate(WinnerTextDisplay,transform);
        newTextObject.transform.localPosition = new Vector3(0,Screen.height/3f,0f);
        TMP_Text winnerText = newTextObject.GetComponent<TMP_Text>();
        winnerText.text = Globals.winningPlayer.username + " Wins!";

        // Create all of the stat displays we need
        for(int i = 0; i < playerList.Count; i++){
            GameObject newStatDisplay = Instantiate(EndStatDisplay,transform);
            displayList.Add(newStatDisplay);
        }

        for(int i = 0; i < displayList.Count;i++){
            Transform displayTransform = displayList[i].transform;
            CardDisplay cardDisplay = displayTransform.GetChild(0).GetComponent<CardDisplay>();
            displayTransform.localPosition = offset;
            
            // Update the card
            cardDisplay.ChangeCardData(playerList[i].card); 
            cardDisplay.player = playerList[i];
            offset.x += 350;

            // Set the stat displays
            TMP_Text stats = displayTransform.GetChild(1).GetComponent<TMP_Text>();
            stats.text =
            $"Damage to Players: {playerList[i].totalDamageToOtherPlayers}\n" +
            $"Damage to Gates: {playerList[i].totalDamageToGates}\n" +
            $"Damage to Gatekeeper: {playerList[i].totalDamageToGatekeeper}\n" +
            $"Amount Healed: {playerList[i].totalAmountHealed}\n" +
            $"Damage Taken: {playerList[i].totalDamageTaken}\n" +
            $"Stockade Collected: {playerList[i].totalStockade}\n" +
            $"Battles Started: {playerList[i].battlesStarted}\n";
            stats.fontSize = 22;
        }

    }

    void Awake(){}

    // Update is called once per frame
    void Update() {}
}
