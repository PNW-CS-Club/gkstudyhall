using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives the gate buttons their functionality.
/// </summary>
public class GateButtonActions : MonoBehaviour
{
    [SerializeField] StateMachine stateMachine;


    /// <summary>
    /// Stores the chosen gate and transitions to the next state.
    /// The game must be in a state where <c>CanChooseGate</c> is true; otherwise this does nothing.  
    /// </summary>
    public void ChooseGate(GateSO gate) 
    {
        if (!stateMachine.CurrentState.CanChooseGate) 
            return;

        Debug.Log($"Chose gate: {gate.Color}");
        Globals.chosenGate = gate;
        stateMachine.TransitionTo(stateMachine.attackingGateState);
    }
}
