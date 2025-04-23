using System;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    /// The state where the current player rolls the dice to determine what they will get on their trait roll.
    TraitRoll = 0, 
    
    /// The state where the current player chooses what gate to attack. 
    ChoosingGate = 1, 
    
    /// The state where the player rolls the dice to determine how much damage they do to the gate. 
    AttackingGate = 2, 
    
    /// The state after a player gets a gate to 0 health where they roll the dice to see what effect happens.
    BreakingGate = 3, 
    
    /// The state where the game waits for the current player to choose a card for a trait, a battle, or a gate effect.
    ChoosingPlayer = 4, 
    
    /// The state where the game waits for the current player to roll to attack another player
    Battling = 5, 

    /// The state where a player is picking two gates to swap
    SwappingGates = 6,

    /// The state where a player forces another player to choose a gate
    ForcingGate = 7
}


public static class StateMethods
{
    public static bool CanRoll(this State state) => state switch
    {
        State.TraitRoll => true,
        State.ChoosingGate => false,
        State.AttackingGate => true,
        State.BreakingGate => true,
        State.ChoosingPlayer => false,
        State.Battling => true,
        State.SwappingGates => false,
        State.ForcingGate => false,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
    
    public static bool CanChooseGate(this State state) => state switch
    {
        State.TraitRoll => false,
        State.ChoosingGate => true,
        State.AttackingGate => false,
        State.BreakingGate => false,
        State.ChoosingPlayer => false,
        State.Battling => false,
        State.SwappingGates => true,
        State.ForcingGate => true,
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}
