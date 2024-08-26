using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IState
{
    // Called when the state machine changes to this state.
    public void Enter() { }

    // Called each frame that this state is the current state.
    public void Update() { }

    // Called when the state machine changes from this state to another one.
    public void Exit() { }
}

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


/*
[Serializable]
public class AttackingState: IState
{
    List<PlayerSO> players;
    Gate the;
    int roll;
    public AttackingState(List<PlayerSO> players, Gate the){
        this.players = players;
        this.the = the;

    }
    public void Enter(){
        roll = DiceRoll.roll;
        GameManager.GateChangeHealth(players[0],the,roll);

    }

     public void Exit() => Debug.Log("Testing TraitRollState");
    
}*/

