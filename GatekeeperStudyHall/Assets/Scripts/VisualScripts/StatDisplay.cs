using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    TMP_Text textbox;

    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponent<TMP_Text>();
        textbox.text = 
            $"Total Damage Dealt to Other Players: {Globals.winningPlayer.totalDamageToOtherPlayers}\n" +
            $"Total Damage Dealt to Gates: {Globals.winningPlayer.totalDamageToGates}\n" +
            $"Total Damage Dealt to Gatekeeper: {Globals.winningPlayer.totalDamageToGatekeeper}\n" +
            $"Total Amount Healed: {Globals.winningPlayer.totalAmountHealed}\n" +
            $"Total Damage Taken: {Globals.winningPlayer.totalDamageTaken}\n" +
            $"Total Stockade Collected: {Globals.winningPlayer.totalStockade}\n" +
            $"Battles Started: {Globals.winningPlayer.battlesStarted}\n";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
