using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.

public enum RandomEvent
{
[InspectorName("Deal one damage to player")]    pDmg = 0;
[InspectorName("Gain a shield at the start of the round")] overShield = 1;
[InspectorName("Add 1 dmg to gate attack roll")] gateAttack = 2;
} //numbers are the index


public static class SpecialEvents
{

List<PlayerSO> players;

int randomNum = Random.Range(0, 100); //

RandomEvent randEvent = Random.Range(0, 3);

public static void specialEvent(PlayerSO player){ 
    Debug.Log("START OF SPECIAL EVENT");
    if(randomNum < 10){ //this will be a special round 
        switch(randEvent){
            case RandomEvent.pDmg:
            player.TakeDamage(1);
            Debug.Log("All player take damage");
            break;

            case RandomEvent.overShield:
            player.hasStockade = true;
            Debug.Log("Random player will get shield");
            break;

            case RandomEvent.gateAttack:
            player.increaseGateDamage = 1;
            Debug.Log("Player does 1 dmg to gate attack roll")
            break;

            default:
            // We shouldn't ever reach this statement.
            Debug.LogError($"Event not handled: {RandomEvent}");
            break;
        }
    }
}

//enum using a switch
//enum will have the effects
//

}