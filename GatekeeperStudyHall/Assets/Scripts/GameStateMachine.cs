using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum GameState
{
    ROLLING_TRAIT = 0,
    CHOOSING_GATE = 1,
    ATTACKING_GATE = 2,
    ROLLING_GATE_EFFECT = 3,
    BATTLING = 4,
}


public class GameStateMachine : MonoBehaviour
{
    // this is a property that lets other classes check the game state with `myStateMachine.State`
    // cannot be set externally due to `private set`
    public GameState State { get; private set; } = GameState.ROLLING_TRAIT;

    // this is how the state can be changed externally, separated into this function
    // because there will likely be lots of side effects to handle
    public void SetState(GameState newState) {
        State = newState;
        Debug.Log("The new state is " + newState.ToString());
    }
    // use this wrapper function to call from an event defined
    // in the editor because enums dont work for some reason
    public void SetState(GameStateObject gso) {
        SetState(gso.gameState);
    }
}
