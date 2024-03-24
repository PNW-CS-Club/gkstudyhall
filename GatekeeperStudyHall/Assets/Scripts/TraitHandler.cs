using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnumExtension
{
    // Define an extension method in a non-nested static class.
    public static class Extensions
    {
        public static void changeHealth(Player p, int amount) //are we allowed to use a Player object as a parameter? 
        {
            // We should have a player method where they can change their health
            //We use that method and change it according to the parameter amount.
        }
        public static void changeGateHealth(Gate g, int amount)
        {
            //Pretty similar to player. I assume we are going to have gate objects.

        }
    }

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
        //This would have to be the trait of the players card which we will need to know
        public Trait myTrait = Trait.deal3Dam;
        public void ActivateTraitBehavior()
        {
            switch (myTrait)
            {
                case Trait.deal3Dam:
                    //Selected players health -3

                    /*
                     * Idk how we wanna go with the selection process.
                     * Maybe pulling up a list of the other plays and their health 
                     * or we can just make it so you can click the card of the player that
                     * wants to be dealt the damage.I dont know how hard that would be to code
                     * 
                     */
                    Extensions.changeHealth(3);
                    Debug.Log("Trait A");
                    break;
                case Trait.minus2gate:
                    //Selcted gates health -2

                    //Gate.changeHealth(-2)????
                    Debug.Log("Trait B");
                    break;
                case Trait.plus1Health:
                    //Change current players health
                    Extensions.changeHealth(1);//This would be how one of the extension methods could be used
                    Debug.Log("Trait C");
                    break;
                case Trait.doubleGateAbil:
                    //I dont really know what to do here tbh;
                    break;
                case Trait.deal2Dam:
                    Extensions.changeHealth(2);
                    break;
                case Trait.reduceGateDamage:
                    Extensions.changeGateHealth(-2); //For this we are gonna have to keep track of what the number is that they rolled and then just do minus 2 to it;
                                                     //Also have to check for the abilities of that gate.
                    break;
                case Trait.noDamageTurn:
                    Extensions.changeHealth(0);
                    break;
                case Trait.swapGateHP();
                    break;
                default:
                    //I don't think we would ever reach this statement.
                    Debug.Log("Invalid Trait");
                    break;
            }
        }
    }
}