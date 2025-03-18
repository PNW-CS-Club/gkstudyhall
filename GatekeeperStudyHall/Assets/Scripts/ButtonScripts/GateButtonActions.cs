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
    public void ChooseGate( GateSO gate ) {
        PlayerSO ply = gameManager.iPlayerListSO.list[ 0 ];

        if (!gameManager.currentState.CanChooseGate()) 
            return;

        if ( ply.forcedGate != gate.Color && 
            ply.forcedGate != GateColor.INVALID &&
            gameManager.currentState == State.ChoosingGate )
            return;

        Debug.Log($"Chose gate: {gate.Color}");

        // player direct attack, cut short afterwards
        if ( ply.directAttack ) {
            gameManager.GateChangeHealth( ply, gate, -1 );
            ply.directAttack = false;
            gameManager.currentState = State.ChoosingGate;
            return;
        }

        // is this redundant, stupid, or both?  too tired to care right now
        if ( ( Globals.selectedGate == null && gameManager.currentState == State.SwappingGates ) || gameManager.currentState != State.SwappingGates ) {
            Globals.selectedGate = gate;
        } else {
            Globals.swapGate = gate;
        }

        if ( gameManager.currentState == State.ForcingGate ) {
            Debug.Log( "FORCED!" );
            Globals.forcedPlayer.forcedGate = gate.Color;
            gameManager.currentState = State.ChoosingGate;

            Globals.selectedGate = null;
        } else if ( gameManager.currentState == State.SwappingGates && Globals.swapGate != null && Globals.selectedGate != Globals.swapGate ) {
            int swapHP = Globals.swapGate.Health;

            Globals.swapGate.SetHealth( Globals.selectedGate.Health );
            Globals.selectedGate.SetHealth( swapHP );
            
            Globals.selectedGate = null;
            Globals.swapGate = null;
            gameManager.currentState = State.ChoosingGate;
        } else if ( gameManager.currentState != State.SwappingGates && Globals.swapGate == null ) {
            gameManager.currentState = State.AttackingGate;
            gameManager.iPlayerListSO.list[ 0 ].forcedGate = GateColor.INVALID;
        }
    }
}
