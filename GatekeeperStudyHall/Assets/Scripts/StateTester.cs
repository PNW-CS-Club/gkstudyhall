using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class StateTester : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    [SerializeField] StateMachine stateMachine;

    void Start() 
    {
        stateMachine = new StateMachine(playerListObject.list);
        stateMachine.Initialize(stateMachine.traitRollState);
    }

    void Update() 
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) {
            stateMachine.TransitionTo(stateMachine.traitRollState);
        }

        stateMachine.Update();
    }
}
