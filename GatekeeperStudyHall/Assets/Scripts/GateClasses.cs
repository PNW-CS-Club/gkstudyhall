using System;
using System.Collections.Generic;

public enum GateColor {
    BLACK, GREEN, RED, BLUE
}

public class Gate {
    // magic numbers are evil!
    public static int MAX_HEALTH = 6;

    // warning:  this is gross!
    // call these like Gate.GateRoll[ gateInstance.gateColor ].odd( playerInstance )
    public static List< ( Action < Gate, PlayerInfo > odd, Action< Gate, PlayerInfo > even ) > GateRoll = new List< ( Action < Gate, PlayerInfo > odd, Action< Gate, PlayerInfo > even ) > {
        // 0 = BLACK
        ( odd: delegate( Gate gate, PlayerInfo ply ) {
            ply.AttackGate( gate, 4 );
        }, even: delegate( Gate gate, PlayerInfo ply ) {
            ply.TakeDamage( 4 );
        } ),

        // 1 = GREEN
        ( odd: delegate( Gate gate, PlayerInfo ply ) {
            ply.AddHealth( 3 );
        }, even: delegate( Gate gate, PlayerInfo ply ) {
            // NOTIMPL: Center gate gains 3 HP
        } ),

        // 2 = RED
        ( odd: delegate( Gate gate, PlayerInfo ply ) {
            ply.doubleDamageToCenter = true;
        }, even: delegate( Gate gate, PlayerInfo ply ) {
            ply.doubleDamageToSelf = true;
        } ),

        // 3 = BLUE
        ( odd: delegate( Gate gate, PlayerInfo ply ) {
            ply.GiveShield();
        }, even: delegate( Gate gate, PlayerInfo ply ) {} )
    };

 

    // store gate color here.  use GateColor.BLACK etc
    public readonly GateColor gateColor;

    // store actions for rolls and when it breaks
    // ngl i don't know when these are called but combining the image
    // of the game board and the issue description resulted in this
    private readonly Action< PlayerInfo > onBreak;

    private int health = Gate.MAX_HEALTH;

    // ctor, for now just set the color
    public Gate( GateColor gateColor ) {
        this.gateColor = gateColor;
    }

    // this shouldn't be used to deal damage, but may be used
    // in "meta" situations, if any ever arise
    public int Health() { get; set; } = this.Gate.MAX_HEALTH;

    // called when the gate takes damage
    // calls its own onBreak action and resets its health
    // as far as i know, that's all that needs to happen
    // returns whether or not the gate was destroyed this damage,
    // maybe for use later
    public bool TakeDamage( PlayerInfo ply, int damage ) {
        health -= damage;

        if ( health <= 0 ) {
            //this.onBreak( ply );
            this.SetHealth( Gate.MAX_HEALTH );
            return true;
        }

        return false;
    }

}