using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StateMachine
{
    List<PlayerSO> players;

    public TraitRollState traitRollState;
    public ChoosingGateState choosingGateState;
    public AttackingGateState attackingGateState;

    // avoid invoking this directly
    [HideInInspector] public event EventHandler<IState> stateChangedEvent; 

    IState currentState;
    public IState CurrentState { get => currentState; }


    public void Initialize(List<PlayerSO> players, IState state) 
    {
        this.players = players;

        traitRollState.Initialize(players);
        choosingGateState.Initialize(players);

        currentState = state;
        OnStateChanged(state);
        currentState.Enter();
    }

    public void TransitionTo(IState state) 
    {
        currentState.Exit();
        currentState = state;
        OnStateChanged(state);
        currentState.Enter();
    }

    public void Update() 
    {
        currentState.Update();
    }


    // call this function instead of using `stateChangedEvent.Invoke(this, state);`
    protected virtual void OnStateChanged(IState state) 
    {
        // this line is important to avoid some sort of race condition
        var handler = stateChangedEvent;

        if (handler != null) {
            handler(this, state);
        }
    }
}

