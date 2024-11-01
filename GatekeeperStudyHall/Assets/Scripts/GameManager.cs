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

    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] CardQueue cardQueue;
    // we can make any of these methods non-static if needed


    void RollEventHandler(object sender, int roll) => UseRollResult(roll);
    void OnEnable() => diceRoll.DoneRollingEvent += RollEventHandler;
    void OnDestroy() => diceRoll.DoneRollingEvent -= RollEventHandler;


    /// <summary>
    /// Plays out the effects of one player attacking another player.
    /// </summary>
    public static void PlayerAttacksPlayer(PlayerSO attacker, PlayerSO defender, int damage)
    {
        if(defender.hasStockade){
            damage = 0;
            defender.hasStockade = false;
            Debug.Log("Stockade blocked the attack!");
        }
        defender.health -= damage;
        Debug.Log($"{attacker.name} attacked {defender.name} for {damage} damage!");

        if (defender.health <= 0) {
            defender.isAlive = false;
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
        if(amount < 0 && player.hasStockade){
            player.hasStockade = false;
            amount = 0;
            Debug.Log("Stockade blocked the damage!");
        }
        player.health += amount;

        if (player.health <= 0) 
        {
            player.health = 0;
            player.isAlive = false;
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
                traitHandler.ActivateCurrentPlayerTrait(roll);
                stateMachine.TransitionTo(stateMachine.choosingGateState);
            }
            else if (roll == 5) 
            {
                // Player rolls a 5, initiate battle with another player
                Debug.Log("(TODO: Implement battling with another player)");
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
            if(currentPlayer.reduceGateDamage == true) {
                int reducedRoll = roll - 2 > 0 ? roll - 2 : 0;
                Debug.Log($"attacking for {reducedRoll} damage");
                gateIsBreaking = GateChangeHealth(currentPlayer, Globals.chosenGate, -reducedRoll);
                currentPlayer.reduceGateDamage = false;
            }
            else {
                Debug.Log($"attacking for {roll} damage");
                gateIsBreaking = GateChangeHealth(currentPlayer, Globals.chosenGate, -roll);
            }
         
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
            // Hi welcome to Chili's
            Debug.Log("hi welcome to chilis");

            Globals.chosenGate.DoBreakEffect(currentPlayer, roll);
            Globals.chosenGate.health = GateSO.STARTING_HEALTH;
            NextTurn();
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
        Globals.chosenGate = null;

        List<PlayerSO>players = playerListSO.list;
        do {      
            players.Insert(players.Count, players[0]);
            players.RemoveAt(0);
        } 
        while (!players[0].isAlive); // TODO: This will loop infinitely if all players are dead

        cardQueue.RepositionCards();

        stateMachine.TransitionTo(stateMachine.traitRollState);
    }
}
