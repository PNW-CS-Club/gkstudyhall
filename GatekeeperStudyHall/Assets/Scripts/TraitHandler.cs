using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// warning: changing enum int values here causes them to desync in the editor
public enum Trait
{
    [InspectorName("Deal 3 Damage")]                deal3Dam = 0,
    [InspectorName("-2 to Gate Roll")]              minus2gate = 1,
    [InspectorName("+1 Health")]                    plus1Health = 2,
    [InspectorName("x2 Gate Ability")]              doubleGateAbil = 3,
    [InspectorName("Deal 2 Damage")]                deal2Dam = 4,
    [InspectorName("reduceGateDamage (??)")]        reduceGateDamage = 5,
    [InspectorName("Take No Damage This Turn")]     noDamageTurn = 6,
    [InspectorName("Swap Gate HP")]                 swapGateHP = 7,
    [InspectorName("Gate Loses 1 HP")]              gateLoses1HP = 8,
    [InspectorName("Lose 2 Health")]                minus2HP = 9,
    [InspectorName("Deal 1 Damage to Everyone")]    allMinus1HP = 10,
    [InspectorName("Choose Gate for Other Player")] chooseGateForOp = 11,
    [InspectorName("Gain Stockade")]                plusStockade = 12,
}

// static classes can't be instantiated
public static class TraitHandler
{
    //[SerializeField] PlayerListSO playerListObject;
    //List<PlayerSO> playerList = playerListObject.list; // refers to list in playerListObject

    public static void ActivateTrait(PlayerSO player, int roll)
    {
        // Trait trait = player.card.traits[roll];
        Trait trait = Trait.plus1Health;

        switch (trait)
        {
            case Trait.deal3Dam:
                //Selected players health -3

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
                //Selcted gates health -2
                //changeGateHealth(player, selectedGate, -2);
                Debug.LogWarning("Trait doubleGateAbil not implemented");
                break;

            case Trait.plus1Health:
                GameManager.PlayerChangeHealth(player, 1);
                break;

            case Trait.doubleGateAbil:
                Debug.LogWarning("Trait doubleGateAbil not implemented");
                break;

            case Trait.deal2Dam:
                //changeHealth(gate,2);//Need parameter.
                Debug.LogWarning("Trait deal2Dam not implemented");
                break;

            case Trait.reduceGateDamage:
                // changeGateHealth(gate,-2); 
                //For this we are gonna have to keep track of
                //what number they rolled and then just do minus 2 to it;
                //Also have to check for the abilities of that gate.
                Debug.LogWarning("Trait reduceGateDamage not implemented");
                break;

            case Trait.noDamageTurn:
                Debug.LogWarning("Trait noDamageTurn not implemented");
                break;

            case Trait.swapGateHP:
                Debug.LogWarning("Trait swapGateHP not implemented");
                break;

            case Trait.gateLoses1HP:
                Debug.LogWarning("Trait gateLoses1HP not implemented");
                break;

            case Trait.minus2HP:
                GameManager.PlayerChangeHealth(player, -2);
                break;

            case Trait.allMinus1HP:
                //The current player's index should be 0
                /*for(int i = 1; i < playerList.Count; i++){
                    playerList[i] = GameManager.PlayerAttacksPlayer(playerList[0], playerList[i] , -1);
                }*/
                break;

            case Trait.chooseGateForOp:
                Debug.LogWarning("Trait chooseGateForOp not implemented");
                break;

            case Trait.plusStockade:
                player.hasStockade = true;
                break;

            default:
                //We shouldn't ever reach this statement.
                Debug.LogError($"Trait not handled: {trait}");
                break;
        }
    }
}