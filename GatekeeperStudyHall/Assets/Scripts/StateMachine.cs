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

    IState currentState;
    public IState CurrentState { get => currentState; }

    public StateMachine(List<PlayerSO> players) 
    {
        this.players = players;

        // TODO: instances of these states already exist because they are serializable,
        // use an initialize method to give them access to `players` instead of overwriting them with new
        traitRollState = new(this.players);
        choosingGateState = new(this.players);
        attackingGateState = new();
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

