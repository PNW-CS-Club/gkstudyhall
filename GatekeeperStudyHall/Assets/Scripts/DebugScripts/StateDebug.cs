using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateDebug : MonoBehaviour
{
    [SerializeField] StateMachine stateMachine;

    TMP_Text textbox;

    private void Start()
    {
        textbox = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textbox.text = 
            $"Current State: {stateMachine.CurrentState.GetType()}\nCan Roll: {stateMachine.CurrentState.CanRoll}\n" 
            + $"Can Choose Gate: {stateMachine.CurrentState.CanChooseGate}\nChosen Gate: {Globals.chosenGate}";
    }
}
