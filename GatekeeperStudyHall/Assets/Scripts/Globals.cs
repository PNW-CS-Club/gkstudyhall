using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// The different types of multiplayer connections that a game can use.
public enum MultiplayerType {
    Offline = 0, LocalNetwork = 1, Remote = 2,
}


/// Holds references to any objects we want easy access to. 
/// Cannot be instantiated; instead the references are stored in static fields.
public static class Globals
{
    /// The type of multiplayer connection currently being used.
    public static MultiplayerType multiplayerType = MultiplayerType.Offline;
    
    /// The gate that the current player has chosen to attack. 
    /// Holds <c>null</c> if the player has not yet chosen a gate or has already attacked a gate.
    public static GateSO selectedGate = null;
    
    ///  A structure of data relevant to battles.
    ///  <c>data</c> needs to be init'd so that it's technically a valid object.
    public struct BattleData {
        public static void Reset() {
            data = new();
            isAttackerRolling = false;
            mult = 1;
        }

        public static List<(PlayerSO player, int roll)> data = new();
        public static bool isAttackerRolling;
        public static int mult;
    }

    /// The number of players still alive 
    public static int playersAlive = -1;

    /// The player who won the game
    /// (only valid after a player has won and before the next game starts)
    public static PlayerSO winningPlayer;
}
