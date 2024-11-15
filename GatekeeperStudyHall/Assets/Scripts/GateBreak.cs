using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBreak : MonoBehaviour
{
    GameManager gameManager;
    StateMachine stateMachine;
    
    void Start()
    {
        GameObject gmObject = GameObject.FindWithTag("GameManager");
        gameManager = gmObject.GetComponent<GameManager>();
        stateMachine = gmObject.GetComponent<StateMachine>();
    }

    
    /// <summary>
    /// This should be called after this gate has been broken
    /// and a player has rolled even or odd.
    /// </summary>
    public IState DoBreakEffect(PlayerSO player, GateSO gate, int roll) 
    {
        return (roll % 2 == 0) ? DoNegativeEffect(player, gate, roll) : DoPositiveEffect(player, gate, roll);
    }
    
    
    IState DoPositiveEffect(PlayerSO player, GateSO gate, int roll)
    {
        switch (gate.Color) 
        {
            case GateColor.BLACK:
                /* TODO: damage another player by 4 health*/ 
                break;

            case GateColor.GREEN:
                GameManager.PlayerChangeHealth(player, 3 * player.doubleGateAbil);
                return null;

            case GateColor.RED:
                player.doubleDamageToCenter = 2; 
                // TODO: damage center for double the roll
                break;

            case GateColor.BLUE:
                player.hasStockade = true;
                return null;
        }

        return null;
    }

    IState DoNegativeEffect(PlayerSO player, GateSO gate, int roll)
    {
        switch (gate.Color) 
        {
            case GateColor.BLACK:
                GameManager.PlayerChangeHealth(player, -4 * player.doubleDamageToSelf * player.doubleGateAbil);
                return null;

            case GateColor.GREEN:
                /* TODO: center gate gains 3 hp */
                return null;

            case GateColor.RED:
                player.doubleDamageToSelf = 2;
                GameManager.PlayerChangeHealth(player, -roll * player.doubleDamageToSelf);
                return null;

            case GateColor.BLUE:
                /* TODO: center gate gets sheild */ 
                return null;
        }

        return null;
    }

}
