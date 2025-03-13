using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.

public enum RandomEvent
{
[InspectorName("Deal one damage to player")]    pDmg = 1;
[InspectorName("Gain a shield at the start of the round")] overShield = 5;
[InspectorName("Add 1 to gate attack roll")] gateAttack = 1;
}


public class Event : MonoBehavior
{

List<PlayerSO> players;

float randomNum = Random.Range(0.0f, 100.0f);

public State Event(PlayerSO player){
    if(randomNum < 30.0f){ //this will be a special round 
        switch(RandomEvent){
            case RandomEvent.pDmg:
            player.damage(1);
            break;

            case RandomEvent.overShield:
            player.shield;
            break;

            case RandomEvent.gateAttack:
            
        }
    }
}

//enum using a switch
//enum will have the effects
//

}