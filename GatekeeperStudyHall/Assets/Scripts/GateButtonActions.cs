using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateButtonActions : MonoBehaviour
{
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

    public void Initialize(StateMachine stateMachine) 
    {
        this.stateMachine = stateMachine;
    }


    public void ChooseGate(Gate gate) 
    {
        if (stateMachine.CurrentState != stateMachine.choosingGateState) 
            return;

        Debug.Log($"Chose gate: {gate.color}");
        Globals.chosenGate = gate;
        stateMachine.TransitionTo(stateMachine.attackingGateState);
    }


    public void ChooseRedGate() => ChooseGate(redGate);
    public void ChooseBlueGate() => ChooseGate(blueGate);
    public void ChooseGreenGate() => ChooseGate(greenGate);
    public void ChooseBlackGate() => ChooseGate(blackGate);
}
