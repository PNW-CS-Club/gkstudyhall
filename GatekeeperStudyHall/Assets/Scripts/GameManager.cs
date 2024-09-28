using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides methods for actions related to health, damage, and turn flow.
/// </summary>
// TODO: if this is the "Game Manager" then why isn't it at the top of the call hierarchy?
public class GameManager : MonoBehaviour
{
    [SerializeField] StateMachine stateMachine;

    [SerializeField] PlayerListSO playerListSO;
    [SerializeField] CardQueue cardQueue;
    // we can make any of these methods non-static if needed

    /// <summary>
    /// Plays out the effects of one player attacking another player.
    /// </summary>
    public static void PlayerAttacksPlayer(PlayerSO attacker, PlayerSO defender, int damage)
    {
        defender.health -= damage;

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
    public static void GateChangeHealth(PlayerSO player, GateSO gate, int amount) 
    {
        gate.health += amount;

        if (gate.health <= 0) 
        {
            // TODO: wait for player to roll again, then break gate (do this with a new state!)
            int roll = Random.Range(1, 7);
            Debug.Log($"TEMP: Random number for gate effect: {roll}");
            gate.DoBreakEffect(player, roll);
            gate.health = GateSO.STARTING_HEALTH;
        }
        else if (gate.health > GateSO.MAX_HEALTH) 
        {
            gate.health = GateSO.MAX_HEALTH;
        }
    }


    /// <summary>
    /// Rotates the player list so that the next person up is in position zero. 
    /// This makes them the "current player."
    /// Then updates the card queue to visually match the new player list orientation.
    /// </summary>
    public void NextTurn() 
    {
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
