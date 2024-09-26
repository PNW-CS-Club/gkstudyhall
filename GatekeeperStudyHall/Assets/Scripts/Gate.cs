using System;
using System.Collections.Generic;

public enum GateColor 
{
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}


/// <summary>
/// Represents a gate with its color and health.
/// </summary>
[Serializable]
public class Gate  // TODO: consider making this a MonoBehavior, SO, or even just a hashmap of health values
{
    public static readonly int STARTING_HEALTH = 6;
    public static readonly int MAX_HEALTH = 6;

    public readonly GateColor color;
    public int health = STARTING_HEALTH;


    public Gate(GateColor color) 
    {
        this.color = color;
    }


    /// <summary>
    /// This should be called after this gate has been broken
    /// and a player has rolled even or odd.
    /// </summary>
    public void DoBreakEffect(PlayerSO player, int roll) 
    {
        if (roll % 2 != 0) 
        {
            // odd roll, positive effect
            switch (color) 
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
            switch (color) 
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