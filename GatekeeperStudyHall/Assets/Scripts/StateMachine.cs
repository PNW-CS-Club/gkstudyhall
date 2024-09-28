using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The <c>StateMachine</c> class holds an <c>IState</c> object that represents the state that the program is in.
/// It also stores and initializes all of the instances of the states.
/// </summary>
public class StateMachine : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListSO;

    public TraitRollState traitRollState;
    public ChoosingGateState choosingGateState;
    public AttackingGateState attackingGateState;

    /// <summary>
    /// This C# event is triggered whenever the state changes.
    /// </summary>
    [HideInInspector] public event EventHandler<IState> StateChangedEvent; 

    // This setup allows code outside this class to read the current state but not write to it.
    public IState CurrentState { get => currentState; }
    private IState currentState;


    /// <summary>
    /// Initializes the states that live in the <c>StateMachine</c>, and then enters the initial state.
    /// </summary>
    void Start() 
    {
        traitRollState.Initialize(playerListSO.list);
        choosingGateState.Initialize(playerListSO.list);

        // the initial state that the program will be in is traitRollState
        currentState = traitRollState;
        StateChangedEvent?.Invoke(this, currentState);
        currentState.Enter();
    }

    void Update() 
    {
        currentState.Update();
    }


    /// <summary>
    /// Transitions from the previous state to the given next state.
    /// Triggers the <c>StateChangedEvent</c>.
    /// </summary>
    public void TransitionTo(IState state) 
    {
        currentState.Exit();
        currentState = state;
        StateChangedEvent?.Invoke(this, currentState);
        currentState.Enter();
    }
}

