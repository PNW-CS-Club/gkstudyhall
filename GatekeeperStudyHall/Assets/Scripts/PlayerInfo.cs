using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    // These are regular fields so that we can inspect them in the editor.
    // Using a boolean for the stockade because it doesn't seem like we'll use the multiple stockade rule.
    public CardData card;
    public int health = 10;
    public bool hasStockade = false;
    static int MAX_HEALTH = 10;

  

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
        health -= damage;
        if ( this.Health <= 0 ) {
            // die func or something else
        }
    }

    // TODO: STUB
    public void AddHealth( int inc ) {
        health += inc;
        // clamp?
    }

    // TODO: STUB
    public void GiveShield() {
        hasStockade = true;
    }
    
   
}
