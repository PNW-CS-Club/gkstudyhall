using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents the different types of traits that cards can have. 
/// </summary>
public enum Trait
{
    // warning: changing enum int values here does not update them in the editor
    [InspectorName("Deal 3 Damage")]                deal3Dam = 0,
    [InspectorName("Deal 2 Damage to Gate")]        minus2gate = 1,
    [InspectorName("+1 Health")]                    plus1Health = 2,
    [InspectorName("x2 Gate Ability")]              doubleGateAbil = 3,
    [InspectorName("Deal 2 Damage")]                deal2Dam = 4,
    [InspectorName("-2 Gate Attack Damage")]        reduceGateDamage = 5,
    [InspectorName("Take No Damage This Turn")]     noDamageTurn = 6,
    [InspectorName("Swap Gate HP")]                 swapGateHP = 7,
    [InspectorName("Gate Loses 1 HP")]              gateLoses1HP = 8,
    [InspectorName("Lose 2 Health")]                minus2HP = 9,
    [InspectorName("Deal 1 Damage to Everyone")]    allMinus1HP = 10,
    [InspectorName("Choose Gate for Other Player")] chooseGateForOp = 11,
    [InspectorName("Gain Stockade")]                plusStockade = 12,
}


/// <summary>
/// Has methods that perform the traits of given players at given rolls.
/// </summary>
public class TraitHandler : MonoBehaviour
{
    [SerializeField] PlayerListSO playerListObject;
    List<PlayerSO> players; // refers to list in playerListObject


    void Start() {
        players = playerListObject.list;
    }

    /// <summary>
    /// Determines the trait of the player whose turn it is at the given roll. 
    /// Then performs the actions that that trait describes.
    /// </summary>
    /// <param name="roll">The roll (1-4) that the trait corresponds to.</param>
    public void ActivateCurrentPlayerTrait(int roll) 
    {
        ActivateTrait(players[0], roll); // TODO: This is throwing an index out of bounds error during the second player's turn
    }


    /// <summary>
    /// Determines the trait to activate using the player and the roll. 
    /// Then performs the actions that that trait describes.
    /// </summary>
    /// <param name="player">The player whose trait is being activated.</param>
    /// <param name="roll">The roll (1-4) that the trait corresponds to.</param>
    public void ActivateTrait(PlayerSO player, int roll)
    {
        if (roll > 4) {
            Debug.LogError("Cannot activate trait with roll more than 4");
            return;
        }

        Trait trait = player.card.traits[roll - 1];

        switch (trait)
        {
            case Trait.deal3Dam:
                //Deal 3 damage to target player

                /*
                 * Idk how we wanna go with the selection process.
                 * Maybe pulling up a list of the other players and their health 
                 * or we can just make it so you can click the card of the player that
                 * wants to be dealt the damage. I dont know how hard that would be to code
                 */
                //changeHealth(player,3);//Need the array of players to access it.
                Debug.LogWarning("Trait deal3Dam not implemented");
                break;

            case Trait.minus2gate:
                //Selected gate health - 2
                //GameManager.GateChangeHealth(player, selectedGate,-2);
                
                Debug.LogWarning("Trait minus2gate not implemented");
                break;

            case Trait.plus1Health:
                GameManager.PlayerChangeHealth(player, 1);
                break;

            case Trait.doubleGateAbil:
                //Double the gate abilities this turn
                player.doubleGateAbil = true; // This will be used in GameManager to carry out Trait ability
                break;

            case Trait.deal2Dam:
                //Deal 2 damage to target player
                Debug.LogWarning("Trait deal2Dam not implemented");
                break;

            case Trait.reduceGateDamage:
                // Player deals 2 less damage to gates this turn
                player.reduceGateDamage = true; // This will be used in GameManager to carry out Trait ability
                break;

            case Trait.noDamageTurn:
                // Player will not take damage this turn
                player.noDamageTurn = true; // This will be used in GameManager to carry out Trait ability
                break;

            case Trait.swapGateHP:
                // Swap the HP of two chosen gates
                Debug.LogWarning("Trait swapGateHP not implemented");
                break;

            case Trait.gateLoses1HP:
                // Select a gate to lose 1 HP
                Debug.LogWarning("Trait gateLoses1HP not implemented");
                break;

            case Trait.minus2HP:
                GameManager.PlayerChangeHealth(player, -2); // Player loses 2 health
                break;

            case Trait.allMinus1HP:
                // Note: the index of the current player is always 0
                for (int i = 1; i < players.Count; i++) {
                    GameManager.PlayerAttacksPlayer(players[0], players[i], 1); // deal 1 damage to all other players
                }
                break;

            case Trait.chooseGateForOp:
                // Choose a gate for another player to attack
                // that player will only be able to attack that gate during their next turn
                Debug.LogWarning("Trait chooseGateForOp not implemented");
                break;

            case Trait.plusStockade:
                player.hasStockade = true;
                break;

            default:
                // We shouldn't ever reach this statement.
                Debug.LogError($"Trait not handled: {trait}");
                break;
        }
    }
}