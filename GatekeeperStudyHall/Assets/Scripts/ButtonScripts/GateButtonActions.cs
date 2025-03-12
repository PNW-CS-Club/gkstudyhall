using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives the gate buttons their functionality.
/// </summary>
public class GateButtonActions : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    
    
    /// <summary>
    /// Stores the chosen gate and transitions to the next state.
    /// The game must be in a state where <c>CanChooseGate</c> is true; otherwise this does nothing.  
    /// </summary>
    public void ChooseGate(GateSO gate) 
    {
        if (!gameManager.currentState.CanChooseGate()) 
            return;

        if ( gameManager.iPlayerListSO.list[ 0 ].forcedGate != gate.Color && gameManager.iPlayerListSO.list[ 0 ].forcedGate != GateColor.INVALID )
            return;

        Debug.Log($"Chose gate: {gate.Color}");

        if ( Globals.selectedGate == null ) {
            Globals.selectedGate = gate;
        } else {
            Globals.swapGate = gate;
        }

        if ( gameManager.currentState == State.ForcingGate ) {
            Debug.Log( "FORCED!" );
            Globals.forcedPlayer.forcedGate = gate.Color;
            gameManager.currentState = State.ChoosingGate;
        } else if ( gameManager.currentState == State.SwappingGates && Globals.swapGate != null && Globals.selectedGate != Globals.swapGate ) {
            int swapHP = Globals.swapGate.Health;

            Globals.swapGate.SetHealth( Globals.selectedGate.Health );
            Globals.selectedGate.SetHealth( swapHP );
            
            Globals.swapGate = null;
            gameManager.currentState = State.ChoosingGate;
        } else {
            gameManager.currentState = State.AttackingGate;
        }

        gameManager.iPlayerListSO.list[ 0 ].forcedGate = GateColor.INVALID;
        Globals.selectedGate = null;
    }
}
