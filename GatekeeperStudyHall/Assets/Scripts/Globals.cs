using System.Collections.Generic;
using UnityEngine;


/// Holds references to any objects we want easy access to. 
/// Cannot be instantiated; instead the references are stored in static fields.
public static class Globals
{
    /// The gate that the current player has chosen to attack. 
    /// Holds <c>null</c> if the player has not yet chosen a gate or has already attacked a gate.
    public static GateSO selectedGate = null;

    // the gate the player is choosing to swap with
    // will use selectedGate as the other half
    public static GateSO swapGate = null;

    // the player whose gate is being forced
    public static PlayerSO forcedPlayer = null;
    
    public class BattleData {
        public PlayerSO attacker;
        public PlayerSO defender;
        public int attackerRoll = 0;
        public int defenderRoll = 0;
        public bool isAttackerRolling;
        public int mult;
        
        public BattleData(PlayerSO attacker, PlayerSO defender) {
            this.attacker = attacker;
            this.defender = defender;
            isAttackerRolling = true;
            mult = 1;
        }
        
        public bool IsTied() => attackerRoll == defenderRoll;
        
        public PlayerSO Winner => (attackerRoll > defenderRoll) ? attacker : defender;
        public PlayerSO Loser => (attackerRoll > defenderRoll) ? defender : attacker;
        
    }
    
    /// Contains all the data that needs to persist through a single battle.
    public static BattleData battleData = null;
    
    /// The number of players still alive
    public static int playersAlive = -1;

    public static PlayerSO winningPlayer;
    public static string winningPlayerUsername;

    public static int sessionMatchesPlayed = 0; //to know if is the first game of the session

    /// A list that contains the players' chosen cards.
    /// Guaranteed to always refer to the same list.
    public static readonly List<CardSO> charSelectCards = new();
}

