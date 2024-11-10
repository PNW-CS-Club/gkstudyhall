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

    /// <summary>
    /// Whether the user can roll the dice while this state is active.
    /// </summary>
    public bool CanRoll { get; }

    /// <summary>
    /// Whether the user can choose a gate while this state is active.
    /// </summary>
    public bool CanChooseGate { get; }
}

/// <summary>
/// The state where the current player rolls the dice to determine what they will get on their trait roll.
/// The current player should be able to interact with the dice, but not the gate buttons.
/// </summary>
[Serializable]
public class TraitRollState : IState
{
    public bool CanRoll => true;
    public bool CanChooseGate => false;
}


/// <summary>
/// The state where the current player chooses what gate to attack. 
/// The current player should be able to interact with the gate buttons, but not the dice.
/// </summary>
[Serializable]
public class ChoosingGateState : IState
{
    public bool CanRoll => false;
    public bool CanChooseGate => true;
}


/// <summary>
/// The state where the player rolls the dice to determine how much damage they do to the gate. 
/// The current player should be able to interact with the dice, but not the gate buttons.
/// </summary>
[Serializable]
public class AttackingGateState : IState
{
    public bool CanRoll => true;
    public bool CanChooseGate => false;
}


/// <summary>
/// The state after a player gets a gate to 0 health where they roll the dice to see what effect happens. 
/// The current player should be able to interact with the dice, but not the gate buttons.
/// </summary>
[Serializable]
public class BreakingGateState : IState
{
    public bool CanRoll => true;
    public bool CanChooseGate => false;
}


/// <summary>
/// The state where the game waits for the current player to choose a card for a trait, a battle, or a gate effect.
/// </summary>
[Serializable]
public class ChoosingPlayerState : IState
{
    [SerializeField] PlayerSelection playerSelect;

    public bool CanRoll => false;
    public bool CanChooseGate => false;

    public void Enter()
    {
        playerSelect.Show();
    }

    public void Exit()
    {
        playerSelect.gameObject.SetActive(false);
    }
}
