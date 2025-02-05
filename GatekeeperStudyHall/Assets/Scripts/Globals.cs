using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds references to any objects we want easy access to. 
/// Cannot be instantiated; instead the references are stored in static fields.
/// </summary>
public static class Globals
{
    /// <summary>
    /// The gate that the current player has chosen to attack. 
    /// Holds <c>null</c> if the player has not yet chosen a gate or has already attacked a gate.
    /// </summary>
    public static GateSO selectedGate = null;

    
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
    
    /// <summary>
    /// Contains all the data that needs to persist through a single battle.
    /// </summary>
    public static BattleData battleData = null;

    /// <summary>
    /// The number of players still alive 
    /// </summary>
    public static int playersAlive = -1;

    public static PlayerSO winningPlayer;

    public static int sessionMatchesPlayed = 0;

    public static List<PlayerSO> playerList;

    
}

