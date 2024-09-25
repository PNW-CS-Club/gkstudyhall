using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class StateTester : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    public StateMachine stateMachine;
    [SerializeField] DiceRoll diceRoll;
    [SerializeField] GateButtonActions gateButtonActions;

    void Start() 
    {
        stateMachine.Initialize(playerListObject.list, stateMachine.traitRollState);
        gateButtonActions.Initialize(stateMachine);
    }

    void Update() 
    {
        stateMachine.Update();
    }
}
