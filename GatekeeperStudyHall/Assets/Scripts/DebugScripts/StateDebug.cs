using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateDebug : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    TMP_Text textbox;

    private void Start()
    {
        textbox = GetComponent<TMP_Text>();
    }

    void Update()
    {
        textbox.text = 
            $"Current State: {gameManager.currentState}\nCan Roll: {gameManager.currentState.CanRoll()}\n" 
            + $"Can Choose Gate: {gameManager.currentState.CanChooseGate()}\nChosen Gates: {Globals.selectedGate} & {Globals.swapGate}";
    }
}
