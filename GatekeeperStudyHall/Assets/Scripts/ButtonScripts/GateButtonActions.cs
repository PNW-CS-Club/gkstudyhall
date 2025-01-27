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

        Debug.Log($"Chose gate: {gate.Color}");
        Globals.selectedGate = gate;
        gameManager.currentState = State.AttackingGate;
    }
}
