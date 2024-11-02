using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_PlayerSO", menuName = "Scriptable Objects/PlayerSO")]
public class PlayerSO : ScriptableObject
{
    public static readonly int STARTING_HEALTH = 10;
    public static readonly int MAX_HEALTH = 10;

    // These are regular fields so that we can inspect them in the editor.
    // Using a boolean for the stockade because it doesn't seem like we'll use the multiple stockade rule.
    public CardSO card;
    public int health;
    public bool hasStockade; 

    public bool isAlive; 

    // not final
    public bool doubleDamageToCenter; // Player deals double damage to center this turn
    public bool doubleDamageToSelf; // Player deals double damage to self this turn
    public bool reduceGateDamage; // Player deals less damage to gate this turn
    public bool increaseGateDamage; // Player deals more damage to gate this turn
    public bool noDamageTurn; // Player takes no damage this turn
    public bool doubleGateAbil; // Gate abilities double this turn

    // when this SO is loaded into a scene, reset values that may have been changed
    private void OnEnable() {
        health = STARTING_HEALTH;
        hasStockade = false;

        isAlive = true;

        doubleDamageToCenter = false;
        doubleDamageToSelf = false;
        reduceGateDamage = false;
        increaseGateDamage = false;
        noDamageTurn = false;
        doubleGateAbil = false;
        
    }

    /// <summary>
    /// Resets all effects that the player has.
    /// </summary>
    public void resetEffects(){
        doubleDamageToCenter = false;
        doubleDamageToSelf = false;
        reduceGateDamage = false;
        increaseGateDamage = false;
        noDamageTurn = false;
        doubleGateAbil = false;
    }
}
