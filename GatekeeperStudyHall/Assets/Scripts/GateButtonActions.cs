using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The <c>GateButtonActions</c> class gives the gate buttons their functionality.
/// Its <c>Choose[Color]Gate</c> methods are meant to be called when the gate buttons are pressed.
/// It is also where the instances of the <c>Gate</c> class live.
/// </summary>
public class GateButtonActions : MonoBehaviour
{
    // potentially, these Gates could be stored in Scriptable Objects, where it *may*
    // be easier to access them than asking the GateButtonActions class to supply them

    // currently, in order to access the gate objects, use for example `gateButtonActions.redGate`
    // where `gateButtonActions` is a reference to the GateButtonActions instance in the scene
    Gate redGate;
    Gate blueGate;
    Gate greenGate;
    Gate blackGate;

    StateMachine stateMachine;


    void Start()
    {
        redGate = new Gate(GateColor.RED);
        blueGate = new Gate(GateColor.BLUE);
        greenGate = new Gate(GateColor.GREEN);
        blackGate = new Gate(GateColor.BLACK);
    }

    /// <summary>
    /// Supplies the <c>GateButtonActions</c> instance with a reference to the state machine.
    /// </summary>
    public void Initialize(StateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
    }


    /// <summary>
    /// Stores the chosen gate and transitions to the next state.
    /// The game must be in <c>ChoosingGateState</c>; otherwise this does nothing.  
    /// </summary>
    public void ChooseGate(Gate gate) 
    {
        if (stateMachine.CurrentState != stateMachine.choosingGateState) 
            return;

        Debug.Log($"Chose gate: {gate.color}");
        Globals.chosenGate = gate;
        stateMachine.TransitionTo(stateMachine.attackingGateState);
    }

    // These are the methods that are meant to be called from the buttons.  
    public void ChooseRedGate() => ChooseGate(redGate);
    public void ChooseBlueGate() => ChooseGate(blueGate);
    public void ChooseGreenGate() => ChooseGate(greenGate);
    public void ChooseBlackGate() => ChooseGate(blackGate);
}
