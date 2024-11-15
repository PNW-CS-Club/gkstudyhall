using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides methods for actions related to health, damage, and turn flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] StateMachine stateMachine;
    [SerializeField] TraitHandler traitHandler;
    [SerializeField] DiceRoll diceRoll;
    [SerializeField] GateBreak gateBreak;

    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] CardQueue cardQueue;

    [SerializeField] PlayerSelection playerSelect;
    
    [SerializeField] CenterGateSO centerGate;
    // we can make any of these methods non-static if needed


    void RollEventHandler(object sender, int roll) => UseRollResult(roll);
    void OnEnable() => diceRoll.DoneRollingEvent += RollEventHandler;
    void OnDestroy() => diceRoll.DoneRollingEvent -= RollEventHandler;


    /// <summary>
    /// Plays out the effects of one player attacking another player.
    /// </summary>
    public static void PlayerAttacksPlayer(PlayerSO attacker, PlayerSO defender, int damage)
    {
        if(defender.noDamageTurn){
            damage = 0;
            Debug.Log($"{defender.name} takes no damage this turn!");
        }
        else if(defender.hasStockade){
            damage = 0;
            defender.hasStockade = false;
            Debug.Log("Stockade blocked the attack!");
        }
        defender.health -= damage;
        Debug.Log($"{attacker.name} attacked {defender.name} for {damage} damage!");

        if (defender.health <= 0) {
            defender.isAlive = false;
            Globals.playersAlive--;
        }
    }


    /// <summary>
    /// <para>Reduces the health of the center gate.</para>
    /// <para>If the center gate's health is reduced to zero, the game ends.</para>
    /// </summary>
    /// <param name="attacker">The player attacking the center gate</param>
    /// <param name="damage"></param>
    public void PlayerAttacksCenterGate(PlayerSO attacker, int damage)
    {
        centerGate.health = Mathf.Clamp(centerGate.health - damage, 0, CenterGateSO.MAX_HEALTH);
        if (centerGate.health == 0)
        {
            Debug.Log($"{attacker.card.characterName} wins! End the game here");
        }
    }


    /// <summary>
    /// Changes the health of a player by the specified amount. 
    /// A player's health is clamped between <c>0</c> and <c>PlayerSO.MAX_HEALTH</c>.
    /// Use this method in the scenario that the player should gain or lose health but is not being attacked.
    /// </summary>
    /// <param name="player">The player whose health changes.</param>
    /// <param name="amount">The amount of health to change by (positive to heal, negative to take damage).</param>
    public static void PlayerChangeHealth(PlayerSO player, int amount)
    {
        if(player.noDamageTurn){
            amount = 0;
            Debug.Log($"{player.name} takes no damage this turn!");
        }
        else if(amount < 0 && player.hasStockade){
            player.hasStockade = false;
            amount = 0;
            Debug.Log("Stockade blocked the damage!");
        }
        player.health += amount;

        if (player.health <= 0) 
        {
            player.health = 0;
            player.isAlive = false;
            Globals.playersAlive--;
        }
        else if (player.health > PlayerSO.MAX_HEALTH) 
        {
            player.health = PlayerSO.MAX_HEALTH;
        }
    }


    /// <summary>
    /// Changes the health of a gate by a specified amount.
    /// Use this method whenever a player causes a gate to change health.
    /// </summary>
    /// <param name="player">The player who caused the change.</param>
    /// <param name="amount">The amount to change by (positive to heal, negative to deal damage)</param>
    /// <returns><c>true</c> only if the gate's health has reached zero.</returns>
    public bool GateChangeHealth(PlayerSO player, GateSO gate, int amount) 
    {
        gate.health += amount;
        gate.health = Mathf.Clamp(gate.health, 0, GateSO.MAX_HEALTH);

        return gate.health == 0;
    }


    /// <summary>
    /// Performs the action expected to happen after the dice is done rolling, depending on the state.
    /// </summary>
    /// <param name="roll">The rolled value of the dice.</param>
    private void UseRollResult(int roll) {
        PlayerSO currentPlayer = playerListSO.list[0];
        if (stateMachine.CurrentState == stateMachine.traitRollState) 
        {
            if (roll <= 4) 
            {
                IState nextState = traitHandler.ActivateCurrentPlayerTrait(roll);
                if (!playerListSO.list[0].isAlive){
                    NextTurn(); // If the player dies from their trait, end their turn
                } else {
                    stateMachine.TransitionTo(nextState); // TODO: what if the player dies after choosing another player?
                }
            }
            else if (roll == 5) 
            {
                // Player rolls a 5, initiate battle with another player
                stateMachine.TransitionTo( stateMachine.choosingPlayerState );
                stateMachine.choosingPlayerState.playerSelect.OnSelect = ( defender ) => {
                    Debug.Log( playerListSO.list[ 0 ] );
                    Debug.Log( defender ); 
                    DoBattle( playerListSO.list[ 0 ], defender );
                };
            }
            else 
            {
                // Skip turn
                NextTurn();
            }
        }
        else if (stateMachine.CurrentState == stateMachine.attackingGateState) 
        {
            bool gateIsBreaking;
            int attack = roll + currentPlayer.increaseGateDamage - currentPlayer.reduceGateDamage;
            attack = Mathf.Max(0, attack); // set to 0 if attack comes out negative
            Debug.Log($"attacking for {attack} damage");
            gateIsBreaking = GateChangeHealth(currentPlayer, Globals.selectedGate, -attack);
         
            if (gateIsBreaking) {
                Debug.Log("You broke the gate!");
                stateMachine.TransitionTo(stateMachine.breakingGateState);
            }
            else {
                NextTurn();
            }
        }
        else if (stateMachine.CurrentState == stateMachine.breakingGateState)
        {
            IState nextState = gateBreak.DoBreakEffect(playerListSO.list[0], Globals.selectedGate, roll);
            Globals.selectedGate.health = GateSO.STARTING_HEALTH;
            
            if (nextState == null)
                NextTurn();
            else 
                stateMachine.TransitionTo(nextState);
        }
        else if ( stateMachine.CurrentState == stateMachine.battlingState ) {
            if ( Globals.battleAttackerAttacking ) {
                Globals.battleData[ 0 ] = new( Globals.battleData[ 0 ].ply, roll );
                Globals.battleAttackerAttacking = false;
                Debug.Log( "ATTACKER rolled a " + roll + ", it is now the DEFENDER's turn" );
            } else {
                Globals.battleData[ 1 ] = new( Globals.battleData[ 1 ].ply, roll );
                Debug.Log( "DEFENDER rolled a " + roll );

                if ( Globals.battleData[ 0 ].roll == Globals.battleData[ 1 ].roll ) {
                    Globals.bDmgTurns++;
                    Globals.battleAttackerAttacking = true;
                    Debug.Log( "These rolls were equal...the stakes rise!  Damage is now " + Globals.bDmgTurns + "x!" );
                } else {
                    PlayerSO damageDealer = Globals.battleData[ 0 ].roll > Globals.battleData[ 1 ].roll ? Globals.battleData[ 0 ].ply : Globals.battleData[ 1 ].ply;
                    PlayerSO damageTaker = Globals.battleData[ 0 ].roll > Globals.battleData[ 1 ].roll ? Globals.battleData[ 1 ].ply : Globals.battleData[ 0 ].ply;

                    int damageDealt = Math.Abs( Globals.battleData[ 0 ].roll - Globals.battleData[ 1 ].roll ) * Globals.bDmgTurns;

                    PlayerAttacksPlayer( damageDealer, damageTaker, damageDealt );
                    
                    Debug.Log( "Battle concluded, a player should have taken " + damageDealt + ", next!" );

                    Globals.battleData = new();
                    Globals.bDmgTurns = 1;
                    Globals.battleAttackerAttacking = false; 
                    stateMachine.TransitionTo( stateMachine.choosingGateState ); 
                }
            }
        }
        else 
        {
            Debug.LogError("The player should not be able to roll the dice now!");
        }
    }


    /// <summary>
    /// Rotates the player list so that the next person up is in position zero. 
    /// This makes them the "current player."
    /// Then updates the card queue to visually match the new player list orientation.
    /// </summary>
    public void NextTurn() 
    {
        Globals.selectedGate = null;

        List<PlayerSO>players = playerListSO.list;

        players[0].resetEffects(); //reset all temporary effects the current player may have
        do {      
            players.Insert(players.Count, players[0]);
            players.RemoveAt(0);
        } 
        while (!players[0].isAlive); // TODO: This will loop infinitely if all players are dead

        cardQueue.RepositionCards();

        stateMachine.TransitionTo(stateMachine.traitRollState);
    }

    /// <summary>
    /// Start battling with another player
    /// </summary>
    public void DoBattle( PlayerSO attacker, PlayerSO defender ) {
        Globals.battleData.Add( new( attacker, 0 ) );
        Globals.battleData.Add( new( defender, 0 ) );

        Globals.bDmgTurns = 1;

        stateMachine.TransitionTo( stateMachine.battlingState );

        Debug.Log( "Battle begun with ATTACKER = " + attacker.card.characterName + " vs  DEFENDER = " + defender.card.characterName );

        Globals.battleAttackerAttacking = true;
    }
}
