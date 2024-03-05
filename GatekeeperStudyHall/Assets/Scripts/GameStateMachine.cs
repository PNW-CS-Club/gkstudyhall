using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum GameState
{
    [InspectorName("Rolling Trait")]
    ROLLING_TRAIT = 0,
    [InspectorName("Choosing Gate")]
    CHOOSING_GATE = 1,
    [InspectorName("Attacking Gate")]
    ATTACKING_GATE = 2,
    [InspectorName("Rolling Gate Effect")]
    ROLLING_GATE_EFFECT = 3,
    [InspectorName("Battling")]
    BATTLING = 4,
}


public class GameStateMachine : MonoBehaviour
{
    // this is a property that lets other classes check the game state with `myStateMachine.State`
    // cannot be set externally due to `private set`
    public GameState State { get; private set; } = GameState.ROLLING_TRAIT;
    [SerializeField] TMPro.TMP_Text textBox; // temporary, for demo purposes only

    // this is how the state can be changed externally, separated into this function
    // because there will likely be lots of side effects to handle
    public void SetState(GameState newState) {
        State = newState;
        textBox.text = "Game State: " + newState.ToString();
    }
    // use this wrapper function to call from an event defined
    // in the editor because enums dont work for some reason
    public void SetState(GameStateObject gso) {
        SetState(gso.gameState);
    }
}
