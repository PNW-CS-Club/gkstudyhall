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
    public bool hasStockade;
    
    public int Health => health; // makes Health readonly in other scripts
    [SerializeField] int health; // makes health visible from inspector
    
    public bool isAlive; 

    // Trait variables
    public int doubleDamageToCenter; // Player deals double damage to center this turn  (Multiplier)
    public int doubleDamageToSelf; // Player deals double damage to self this turn      (Multiplier)
    public int reduceGateDamage; // Player deals less damage to gate this turn
    public int increaseGateDamage; // Player deals more damage to gate this turn
    public bool noDamageTurn; // Player takes no damage this turn
    public int doubleGateAbil; // Gate abilities double this turn                       (Multiplier)

    // Statistic variables
    public int totalDamageToOtherPlayers; // Total damage dealt to other players
    public int totalDamageToGates;
    public int totalDamageToGatekeeper;
    public int totalAmountHealed; // Total health healed during the game
    public int totalStockade; // Total amount of stockade collected during the game
    public int battlesStarted; // Total number of battles started

    // when this SO is loaded into a scene, reset values that may have been changed
    private void OnEnable() {
        health = STARTING_HEALTH;
        hasStockade = false;

        isAlive = true;
  
        // Initialize Trait Variables
        doubleDamageToCenter = 1;
        noDamageTurn = false;
        reduceGateDamage = 0;
        increaseGateDamage = 0;
        doubleGateAbil = 1;
        doubleDamageToSelf = 1;

        // Initialize Statistic Variables
        totalDamageToOtherPlayers = 0;
        totalDamageToGates = 0;
        totalDamageToGatekeeper = 0;
        totalAmountHealed = 0;
        totalStockade = 0;
        battlesStarted = 0;
    }

    /// <summary>
    /// This function should only be used to reset a player at the start of a new game.
    /// It resets the player's health, stockade status and all statistics variables.
    /// Make sure it is called for each player in the player list.
    /// </summary>
    public void GameReset() {

        health = STARTING_HEALTH;
        hasStockade = false;

        isAlive = true;

        // Reset all statistics
        totalDamageToOtherPlayers = 0;
        totalDamageToGates = 0;
        totalDamageToGatekeeper = 0;
        totalAmountHealed = 0;
        totalStockade = 0;
        battlesStarted = 0;
        
    }

    /// <summary>
    /// Resets all effects that the player has.
    /// </summary>
    public void ResetEffects(){
        doubleDamageToCenter = 1;
        noDamageTurn = false;
        increaseGateDamage = 0;
        reduceGateDamage = 0;
        doubleGateAbil = 1;
        doubleDamageToSelf = 1;
    }

    /// <summary>
    /// Player's health is subtracted by damage parameter. 
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        if (noDamageTurn) {
            Debug.Log($"{name} takes no damage this turn!");
            return;
        }
        if (hasStockade) {
            hasStockade = false;
            Debug.Log("Stockade blocked the attack!");
            return;
        }
        
        health -= damage;
        
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
            Globals.playersAlive--;
        }
        
    }
    /// <summary>
    /// Player's health is increased by the given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        
        health = Mathf.Min(health + amount, MAX_HEALTH);
    }
}
