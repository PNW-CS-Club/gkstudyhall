using System;
using System.Collections.Generic;

public enum GateColor {
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}


public class Gate {
    
    public const int MAX_HEALTH = 6; // magic numbers are evil!

    public readonly GateColor gateColor;

    private int health = MAX_HEALTH;

    // this shouldn't be used to deal damage, but we will need to access health 
    // in order to display the gate health
    public int Health {
        get { return health; }
        set { health = value; }
    }


    public Gate( GateColor gateColor ) {
        this.gateColor = gateColor;
    }


    // returns whether or not the gate was destroyed by this damage
    public bool TakeDamage(PlayerInfo player, int damage) {
        health -= damage;

        if (health <= 0) {
            return true;
        }

        return false;

        // TODO: wait for player to roll again, then break gate 
        // we can do that outside of this function
    }


    // this should be called after this gate has been broken
    // and a player has rolled even or odd
    public void DoBreakEffect(PlayerInfo player, int roll) {
        if (roll % 2 != 0) {
            // odd roll, positive effect
            switch (gateColor) {
                case GateColor.BLACK:
                    /* TODO: damage center gate by 4 */ break;

                case GateColor.GREEN:
                    player.AddHealth(3); break;

                case GateColor.RED:
                    player.doubleDamageToCenter = true; break;

                case GateColor.BLUE:
                    player.GiveShield(); break;
            }
        }

        else {
            // even roll, negative effect
            switch (gateColor) {
                case GateColor.BLACK:
                    player.TakeDamage(4); break;

                case GateColor.GREEN:
                    /* TODO: center gate gains 3 hp */ break;

                case GateColor.RED:
                    player.doubleDamageToSelf = true; break;

                case GateColor.BLUE:
                    /* TODO: center gate gets sheild */ break;
            }
        }
    }

}