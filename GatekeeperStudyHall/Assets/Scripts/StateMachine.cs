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
    // public AttackingState attackingState;

    IState currentState;
    public IState CurrentState { get => currentState; }

    public StateMachine(List<PlayerSO> players) 
    {
        this.players = players;

        traitRollState = new(players);
        choosingGateState = new(players);
        // attackingState = ...
    }

    public void Initialize(IState state) 
    {
        currentState = state;
        currentState.Enter();
    }

    public void TransitionTo(IState state) 
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void Update() 
    {
        currentState.Update();
    }
}

