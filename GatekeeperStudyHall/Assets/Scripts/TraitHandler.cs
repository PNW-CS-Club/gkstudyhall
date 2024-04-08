using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnumExtension
{
    
    // Define an extension method in a non-nested static class.
    public static class Extensions
    {
        //The getters and setters in player and gate IDK how to call properly can be changed 

        public static void changeHealth(PlayerInfo p, int amount) 
        {
            p.setHealth(p.getHealth()-amount);
        }
        public static void changeGateHealth(GateClasses g, int amount)
        {
            boolean dead = g.TakeDamage(player,amount);
            if(dead == true){
                g.setHealth(0);
            }
            else{
                g.setHealth(g.getHealth()-amount);
            }

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
        private PlayerInfo player = new PlayerInfo();//tester player.
        private Gate pGate = new Gate("BLACK");//tester gateclass.

       public Trait myTrait = Trait.deal3Dam; //This would have to be the trait of the players card which we will need to know

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
                    myTrait.changeHealth(player,3);//Need the array of players to access it.
                    Debug.Log("Trait A");
                    break;

                case Trait.minus2gate:
                    //Selcted gates health -2

                    //Gate.changeHealth(-2)????
                    myTrait.changeGateHealth(gate,-2)
                    Debug.Log("Trait B");
                    break;
                case Trait.plus1Health:
                    //Change current players health
                    myTrait.changeHealth(gate,1);//This would be how one of the extension methods could be used
                    Debug.Log("Trait C");
                    break;
                case Trait.doubleGateAbil:
                    //I dont really know what to do here tbh;

                    break;
                case Trait.deal2Dam:
                    myTrait.changeHealth(gate,2);//Need parameter.
                    break;
                case Trait.reduceGateDamage:
                    myTrait.changeGateHealth(gate,-2); //For this we are gonna have to keep track of what the number is that they rolled and then just do minus 2 to it;
                                                     //Also have to check for the abilities of that gate.
                    break;
                case Trait.noDamageTurn:
                    myTrait.changeHealth(player,0);
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