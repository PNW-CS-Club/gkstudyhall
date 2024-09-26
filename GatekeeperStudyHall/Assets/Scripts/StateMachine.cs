using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StateMachine  // TODO: make this class derive from MonoBehavior
{
    List<PlayerSO> players;

    public TraitRollState traitRollState;
    public ChoosingGateState choosingGateState;
    public AttackingGateState attackingGateState;

    /// <summary>
    /// This event is triggered whenever the state changes. 
    /// You can subscribe and unsubscribe to it freely, but avoid invoking it directly.
    /// </summary>
    [HideInInspector] public event EventHandler<IState> stateChangedEvent; 

    // This setup allows code outside this class to read the current state but not write to it.
    public IState CurrentState { get => currentState; }
    private IState currentState;


    /// <summary>
    /// Because <c>StateMachine</c> is <c>Serializable</c>, instances are created before the game starts, 
    /// so instead of a constructor or Start method, we need this <c>Initialize</c> method to give it its proper initial values.
    /// </summary>
    public void Initialize(List<PlayerSO> players, IState state) 
    {
        this.players = players;

        traitRollState.Initialize(players);
        choosingGateState.Initialize(players);

        currentState = state;
        BroadcastStateWasChanged(state);
        currentState.Enter();
    }

    /// <summary>
    /// Transitions from the previous state to the given next state.
    /// Triggers the <c>stateChangedEvent</c>.
    /// </summary>
    public void TransitionTo(IState state) 
    {
        currentState.Exit();
        currentState = state;
        BroadcastStateWasChanged(state);
        currentState.Enter();
    }

    public void Update() 
    {
        currentState.Update();
    }


    // call this function instead of using `stateChangedEvent.Invoke(this, state);`
    private void BroadcastStateWasChanged(IState state) 
    {
        // this line is important to avoid some sort of race condition
        var broadcaster = stateChangedEvent;

        if (broadcaster != null) {
            broadcaster(this, state);
        }
    }
}

