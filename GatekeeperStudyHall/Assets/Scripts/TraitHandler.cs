using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// warning: changing enum int values here causes them to desync in the editor
public enum Trait
{
    deal3Dam = 0,
    minus2gate = 1,
    plus1Health = 2,
    doubleGateAbil = 3,
    deal2Dam = 4,
    reduceGateDamage = 5,
    noDamageTurn = 6,
    swapGateHP = 7,
    gateLoses1HP = 8,
    minus2HP = 9,
    allMinus1HP = 10,
    chooseGateForOp = 11,
}
    
public class TraitHandler : MonoBehaviour
{
    public void ActivateTrait(PlayerInfo player, int roll)
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

                //Gate.changeHealth(-2)????
                //changeGateHealth(gate, -2);
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
                //For this we are gonna have to keep track of what the number is that they rolled and then just do minus 2 to it;
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
                Debug.LogWarning("Trait allMinus1HP not implemented");
                break;

            case Trait.chooseGateForOp:
                Debug.LogWarning("Trait chooseGateForOp not implemented");
                break;

            default:
                //I don't think we would ever reach this statement.
                Debug.LogError($"Trait not handled: {trait}");
                break;
        }
    }
}