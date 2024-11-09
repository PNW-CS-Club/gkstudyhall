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
    /// The character card that the current player has selected.
    /// Holds <c>null</c> if no card has been selected or if we no longer need a reference to the selected card.
    /// </summary>
    public static CardSO selectedCard = null;

    /// <summary>
    ///   <para>The state that we want to enter after we exit the current state. </para>
    ///   <para>This field is useful when it would otherwise not be clear what state the <see cref="StateMachine" /> should transition to.  </para>
    /// </summary>
    public static IState scheduledState = null;
}
