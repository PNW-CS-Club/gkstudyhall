using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    static int MAX_HEALTH = 10;

    /*
        Stores the following information:

        Player Card
        Health - Initialize to 10 HP
        Stockades - 0 (Player takes no damage when stockade >= 1)

        Maybe TurnOrder / Player Number (Could be handled elsewhere)
    */

    // public Card? PlayerCard { get; private set; } // to be implemented
    public int Health { get; private set; } = PlayerInfo.MAX_HEALTH;
    public int Stockades { get; private set; } = 0;

    // not final
    // discuss:  could use a bitfield depending on how many vars we'll need
    public bool doubleDamageToCenter = false;
    public bool doubleDamageToSelf = false;

    // this definitely conflicts with main branch
    public void AttackGate( Gate gate, int damage ) {
        if ( this.doubleDamageToCenter /* && gate.IsCenterGate() */ ) { // TODO: not implemented
            gate.TakeDamage( this, damage );
            this.doubleDamageToCenter = false;
        }
        gate.TakeDamage( this, damage );
    }

    // TODO: STUB
    public void TakeDamage( int damage ) {
        this.Health -= damage;
        if ( this.Health <= 0 ) {
            // die func or something else
        }
    }

    // TODO: STUB
    public void AddHealth( int health ) {
        this.Health += health;
        // clamp?
    }

    // TODO: STUB
    public void GiveShield() {
        Stockades++;
    }
}
