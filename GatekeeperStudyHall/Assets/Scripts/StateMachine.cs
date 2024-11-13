using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// The <c>StateMachine</c> class holds an <c>IState</c> object that represents the state that the program is in.
/// It also stores all of the instances of the states.
/// </summary>
public class StateMachine : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;

    public TraitRollState traitRollState;
    public ChoosingGateState choosingGateState;
    public AttackingGateState attackingGateState;
    public BreakingGateState breakingGateState;
    public ChoosingPlayerState choosingPlayerState;
    public BattlingState battlingState;

    /// <summary>
    /// This C# event is triggered whenever the state changes.
    /// </summary>
    [HideInInspector] public event EventHandler<IState> StateChangedEvent; 

    public IState CurrentState { get; private set; }


    void Start() 
    {
        // the initial state that the program will be in is traitRollState
        CurrentState = traitRollState;
        StateChangedEvent?.Invoke(this, CurrentState);
        CurrentState.Enter();
    }

    void Update() 
    {
        CurrentState.Update();
    }


    /// <summary>
    /// Transitions from the previous state to the given next state.
    /// Triggers the <c>StateChangedEvent</c>.
    /// </summary>
    public void TransitionTo(IState state) 
    {
        Assert.IsNotNull(state, "The state the machine is entering must have a value.");

        CurrentState.Exit();
        CurrentState = state;
        StateChangedEvent?.Invoke(this, CurrentState);
        CurrentState.Enter();
    }
}

