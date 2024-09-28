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
    public static GateSO chosenGate = null;
}
