using System;
using System.Collections.Generic;
using UnityEngine;

public enum GateColor 
{
    BLACK = 0, GREEN = 1, RED = 2, BLUE = 3
}


/// <summary>
/// Represents a gate with its color and health.
/// </summary>
[CreateAssetMenu(fileName = "New_GateSO", menuName = "Scriptable Objects/GateSO")]
public class GateSO : ScriptableObject
{
    public const int STARTING_HEALTH = 6;
    public const int MAX_HEALTH = 6;

    public GateColor Color { get => color; }
    [SerializeField] GateColor color;

    public int health = STARTING_HEALTH;


    private void OnEnable() 
    {
        health = STARTING_HEALTH;
    }


    public override string ToString() {
        return $"GateSO[health=\"{health}\", color=\"{color}\"]";
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
                    /* TODO: damage another player by 4 health*/ break;

                case GateColor.GREEN:
                    GameManager.PlayerChangeHealth(player, 3 * player.doubleGateAbil);
                    break;

                case GateColor.RED:
                    player.doubleDamageToCenter = 2; 
                    // TODO: damage center for double the roll
                    break;

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
                    GameManager.PlayerChangeHealth(player, -4 * player.doubleDamageToSelf * player.doubleGateAbil );
                    break;

                case GateColor.GREEN:
                    /* TODO: center gate gains 3 hp */ break;

                case GateColor.RED:
                    player.doubleDamageToSelf = 2;
                    GameManager.PlayerChangeHealth(player, -roll * player.doubleDamageToSelf);
                    break;

                case GateColor.BLUE:
                    /* TODO: center gate gets sheild */ break;
            }
        }
    }

}