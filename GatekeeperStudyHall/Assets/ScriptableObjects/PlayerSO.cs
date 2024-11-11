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
    public int doubleDamageToCenter; // Player deals double damage to center this turn  (Multiplier)
    public int doubleDamageToSelf; // Player deals double damage to self this turn      (Multiplier)
    public int reduceGateDamage; // Player deals less damage to gate this turn
    public int increaseGateDamage; // Player deals more damage to gate this turn
    public bool noDamageTurn; // Player takes no damage this turn
    public int doubleGateAbil; // Gate abilities double this turn                       (Multiplier)

    // when this SO is loaded into a scene, reset values that may have been changed
    private void OnEnable() {
        health = STARTING_HEALTH;
        hasStockade = false;

        isAlive = true;
  
        doubleDamageToCenter = 1;
        noDamageTurn = false;
        reduceGateDamage = 0;
        increaseGateDamage = 0;
        doubleGateAbil = 1;
        doubleDamageToSelf = 1;
    }

    /// <summary>
    /// Resets all effects that the player has.
    /// </summary>
    public void resetEffects(){
        doubleDamageToCenter = 1;
        noDamageTurn = false;
        increaseGateDamage = 0;
        reduceGateDamage = 0;
        doubleGateAbil = 1;
        doubleDamageToSelf = 1;
    }
}
