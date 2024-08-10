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
    [SerializeField] int roll = 1; // I didnt know how we are tracking what the player gets when they roll.
    // ^^^ we can use SerializeField here because TraitRollStates are Serializable
    // this is just an example, idk if we want this to be visible in the inspector
    [SerializeField] TraitHandlerSO traitHandler;

    public TraitRollState(List<PlayerSO> players) 
    {
        this.players = players;
    }

        roll = 4;
        traitHandler.ActivateTrait(players[0], roll);
    public void Enter() 
    {
    }

    public void Exit() => Debug.Log("Testing TraitRollState");

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

