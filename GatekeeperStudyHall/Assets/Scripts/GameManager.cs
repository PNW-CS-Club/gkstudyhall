using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Player actions will be handled here
    //Will interact with PlayerInfo and GateClasses

    public static void PlayerAttacksPlayer(PlayerInfo p1, PlayerInfo p2, int amount)
    {
        p2.health -= amount;

        if (p2.health <= 0) {
            p2.isAlive = false;
        }
    }


    //Change player health - Can occur when a trait activates
    public static void PlayerChangeHealth(PlayerInfo player, int amount)
    {
        player.health += amount;

        if (player.health <= 0) 
        {
            player.isAlive = false;
        }
        else if (player.health > PlayerInfo.MAX_HEALTH) 
        {
            player.health = PlayerInfo.MAX_HEALTH;
        }
    }


    // Should be called whenever a player causes a gate to change health (traits / attacking)
    public static void GateChangeHealth(PlayerInfo player, Gate gate, int amount) 
    {
        gate.health += amount;

        if (gate.health <= 0) 
        {
            // TODO: wait for player to roll again, then break gate 
            int roll = Random.Range(1, 7);
            Debug.Log($"TEMP: Random number for gate effect: {roll}");
            gate.DoBreakEffect(player, roll);
            gate.health = Gate.STARTING_HEALTH;
        }
        else if (gate.health > Gate.MAX_HEALTH) 
        {
            gate.health = Gate.MAX_HEALTH;
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
