using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Represents the different types of traits that cards can have. 
/// </summary>
public enum Trait
{
    // warning: changing enum int values here does not update them in the editor
    [InspectorName("Deal 3 Damage")]                deal3Dam = 0,
    [InspectorName("Deal 2 Damage to GateKeeper")]  minus2GateKeeper = 1,
    [InspectorName("+1 Health")]                    plus1Health = 2,
    [InspectorName("x2 Gate Ability")]              doubleGateAbil = 3,
    [InspectorName("Deal 2 Damage")]                deal2Dam = 4,
    [InspectorName("-2 Gate Attack Damage")]        reduceGateDamage = 5,
    [InspectorName("Take No Damage This Turn")]     noDamageTurn = 6,
    [InspectorName("Swap Gate HP")]                 swapGateHP = 7,
    [InspectorName("+1 Gate Attack Damage")]        increaseGateDamage = 8,
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
    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerSelection playerSelection;
    [SerializeField] PlayerListSO playerListObject;

    AbilityDisplay abilityDisplay;
    
    List<PlayerSO> players; // refers to list in playerListObject


    void Start() {
        players = playerListObject.list;
        
        abilityDisplay = FindAnyObjectByType<AbilityDisplay>(FindObjectsInactive.Include);
    }

    /// <summary>
    /// Determines the trait of the player whose turn it is at the given roll. 
    /// Then performs the actions that that trait describes.
    /// </summary>
    /// <param name="roll">The roll (1-4) that the trait corresponds to.</param>
    /// <returns>The state the game should enter after the trait roll is activated.</returns>
    public State ActivateCurrentPlayerTrait(int roll) 
    {
        return ActivateTrait(players[0], roll); 
    }

    /// <summary>
    /// Determines the trait to activate using the player and the roll. 
    /// Then performs the actions that that trait describes.
    /// </summary>
    /// <param name="player">The player whose trait is being activated.</param>
    /// <param name="roll">The roll (1-4) that the trait corresponds to.</param>
    /// <returns>The state the game should enter after the trait roll is activated.</returns>
    public State ActivateTrait(PlayerSO player, int roll)
    {
        Assert.IsTrue(1 <= roll && roll <= 4, "Trait value must be between 1 and 4");

        Trait trait = player.card.traits[roll - 1];
        
        this.abilityDisplay.gameObject.SetActive( true );

        // some branches return early with a special state
        // all others will just return choosing gate state
        switch (trait)
        {
            case Trait.deal3Dam:
                //Deal 3 damage to target player
                Debug.Log("Select a player to deal 3 damage to");
                playerSelection.OnSelect = (selectedPlayer) => {
                    gameManager.PlayerAttacksPlayer(player, selectedPlayer, 3);
                    gameManager.currentState = State.ChoosingGate;
                };

                return State.ChoosingGate;

            case Trait.minus2GateKeeper:
                //Selected gate health - 2
                //GameManager.GateChangeHealth(player, selectedGate,-2);
                
                Debug.LogWarning("Trait minus2GateKeeper not implemented");
                break;

            case Trait.plus1Health:
                gameManager.PlayerChangeHealth(player, 1);
                break;

            case Trait.doubleGateAbil:
                //Double the gate abilities this turn
                player.doubleGateAbil = 2; // doubleGateAbil is a multiplier
                break;

            case Trait.deal2Dam:
                //Deal 2 damage to target player
                Debug.Log("Select a player to deal 2 damage to");
                playerSelection.OnSelect = (selectedPlayer) => {
                    gameManager.PlayerAttacksPlayer(player, selectedPlayer, 2);
                    gameManager.currentState = State.ChoosingGate;
                };

                return State.ChoosingPlayer;

            case Trait.reduceGateDamage:
                // Player deals 2 less damage to gates this turn
                player.reduceGateDamage = 2; // Subtracted from player's gate attack for the turn
                break;

            case Trait.noDamageTurn:
                // Player will not take damage this turn
                player.noDamageTurn = true; // This will be used in GameManager to carry out Trait ability
                break;

            case Trait.swapGateHP:
                // Swap the HP of two chosen gates
                Debug.LogWarning("Trait swapGateHP not implemented");
                break;

            case Trait.increaseGateDamage:
                // Player deals 1 more damage to gates this turn
                player.increaseGateDamage = 1; // Added to player's gate attack for the turn
                break;

            case Trait.minus2HP:
                gameManager.PlayerChangeHealth(player, -2); // Player loses 2 health
                break;

            case Trait.allMinus1HP:
                // Note: the index of the current player is always 0
                for (int i = 1; i < players.Count; i++) {
                    gameManager.PlayerAttacksPlayer(players[0], players[i], 1); // deal 1 damage to all other players
                }
                break;

            case Trait.chooseGateForOp:
                // Choose a gate for another player to attack
                // that player will only be able to attack that gate during their next turn
                Debug.LogWarning("Trait chooseGateForOp not implemented");
                break;

            case Trait.plusStockade:
                player.hasStockade = true;
                player.totalStockade++;
                break;

            default:
                // We shouldn't ever reach this statement.
                Debug.LogError($"Trait not handled: {trait}");
                break;
        }

        return State.ChoosingGate;
    }
}