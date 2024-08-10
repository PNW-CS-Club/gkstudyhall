using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class StateTester : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    [SerializeField] StateMachine stateMachine;
    [SerializeField] DiceRoll diceRoll;

    void Start() 
    {
        stateMachine = new StateMachine(playerListObject.list);
        stateMachine.Initialize(stateMachine.traitRollState);
        diceRoll.Initialize(stateMachine);
    }

    void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            stateMachine.TransitionTo(stateMachine.traitRollState);
        }

        stateMachine.Update();
    }
}
