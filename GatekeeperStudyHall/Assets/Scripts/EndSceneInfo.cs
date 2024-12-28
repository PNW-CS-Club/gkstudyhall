using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSceneInfo : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] CardDisplay cardDisplay1;
    [SerializeField] CardDisplay cardDisplay2;
    [SerializeField] CardDisplay cardDisplay3;
    [SerializeField] CardDisplay cardDisplay4;

    [SerializeField] GameObject statDisplay1;
    [SerializeField] GameObject statDisplay2;
    [SerializeField] GameObject statDisplay3;
    [SerializeField] GameObject statDisplay4;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"{Globals.winningPlayer} won the game!");
                
    }

    void Awake(){
        List<PlayerSO> playerList = playerListSO.list;

        cardDisplay1.cardData = Globals.winningPlayer.card;

        // set the rest of the card displays to the other players
        
        cardDisplay2.cardData = playerList[1].card;
        cardDisplay3.cardData = playerList[2].card;
        cardDisplay4.cardData = playerList[3].card;
        

        statDisplay1.GetComponent<TMP_Text>().text = 
            $"Total Damage Dealt to Other Players: {Globals.winningPlayer.totalDamageToOtherPlayers}\n" +
            $"Total Damage Dealt to Gates: {Globals.winningPlayer.totalDamageToGates}\n" +
            $"Total Damage Dealt to Gatekeeper: {Globals.winningPlayer.totalDamageToGatekeeper}\n" +
            $"Total Amount Healed: {Globals.winningPlayer.totalAmountHealed}\n" +
            $"Total Damage Taken: {Globals.winningPlayer.totalDamageTaken}\n" +
            $"Total Stockade Collected: {Globals.winningPlayer.totalStockade}\n" +
            $"Battles Started: {Globals.winningPlayer.battlesStarted}\n";
    }

    // Update is called once per frame
    void Update() {}
}
