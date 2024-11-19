using System;
using System.Collections;
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

    /// <summary>
    ///  A structure of data relevant to battles.
    ///  <c>data</c> needs to be init'd so that it's technically a valid object.
    /// </summary>
    public struct BattleData {
        public static void Reset() {
            data = new();
            turn = false;
            mult = 1;
        }

        public static List< ( PlayerSO ply, int roll ) > data = new();
        public static bool turn;
        public static int mult;
    }

    /// <summary>
    /// The number of players still alive 
    /// </summary>
    public static int playersAlive = -1; 
}
