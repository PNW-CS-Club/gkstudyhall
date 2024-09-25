using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An interface that any state used in the <c>StateMachine</c> must implement.
/// If any method is not given an implementation, it simply does nothing when called.
/// </summary>
public interface IState
{
    /// <summary>
    /// Called when the state machine changes to this state.
    /// </summary>
    public void Enter() { }

    /// <summary>
    /// Called each frame that this state is active.
    /// </summary>
    public void Update() { }

    /// <summary>
    /// Called when the state machine changes from this state to another one.
    /// </summary>
    public void Exit() { }
}

/// <summary>
/// The state where the current player rolls the dice to determine what they will get on their trait roll.
/// The current player should be able to interact with the dice, but not the gate buttons.
/// </summary>
[Serializable]
public class TraitRollState : IState
{
    List<PlayerSO> players;
    [SerializeField] TraitHandlerSO traitHandler;

    public TraitRollState(List<PlayerSO> players) 
    {
        this.players = players;
    }

    public void Enter() 
    {
        Debug.Log($"It's {players[0].name}'s turn to roll their trait.");
    }
}


/// <summary>
/// The state where the current player chooses what gate to attack. 
/// The current player should be able to interact with the gate buttons, but not the dice.
/// </summary>
[Serializable]
public class ChoosingGateState : IState
{
    List<PlayerSO> players;
    
    public ChoosingGateState(List<PlayerSO> players) 
    {
        this.players = players;
    }

    public void Enter() 
    {
        Debug.Log("Time to choose a gate to attack.");
    }
}


/// <summary>
/// The state where the player rolls the dice to determine how much damage they do to the gate. 
/// The current player should be able to interact with the dice, but not the gate buttons.
/// </summary>
[Serializable]
public class AttackingGateState : IState { }
