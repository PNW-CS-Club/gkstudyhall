using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class StateMachine
{
    List<PlayerSO> players;

    public TraitRollState traitRollState;
    // public AttackingState attackingState;

    IState currentState;

    public void Initialize(List<PlayerSO> players, IState state) {
        this.players = players;

        traitRollState.Initialize(players);
        // attackingState ...

        currentState = state;
        currentState.Enter();
    }

    public void TransitionTo(IState state) {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void Update() {
        currentState.Update();
    }
}

