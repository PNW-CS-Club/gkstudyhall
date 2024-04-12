using System;
using System.Collections.Generic;

public enum GateColor 
{
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}

public class Gate 
{
    public static readonly int STARTING_HEALTH = 6;
    public static readonly int MAX_HEALTH = 6;

    public readonly GateColor gateColor;

    // if the player is being attacked, change their health through the GameManager methods
    public int health = STARTING_HEALTH;


    public Gate(GateColor gateColor) 
    {
        this.gateColor = gateColor;
    }


    // this should be called after this gate has been broken
    // and a player has rolled even or odd
    public void DoBreakEffect(PlayerInfo player, int roll) 
    {
        if (roll % 2 != 0) 
        {
            // odd roll, positive effect
            switch (gateColor) 
            {
                case GateColor.BLACK:
                    /* TODO: damage center gate by 4 */ break;

                case GateColor.GREEN:
                    GameManager.PlayerChangeHealth(player, 3); break;

                case GateColor.RED:
                    player.doubleDamageToCenter = true; break;

                case GateColor.BLUE:
                    player.hasStockade = true; break;
            }
        }
        else 
        {
            // even roll, negative effect
            switch (gateColor) 
            {
                case GateColor.BLACK:
                    GameManager.PlayerChangeHealth(player, -4); break;

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