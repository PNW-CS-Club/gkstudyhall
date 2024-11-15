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
    /// <returns>The next state the player should enter this turn, or null if the turn should end.</returns>
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
                return stateMachine.choosingGateState;

            case GateColor.GREEN:
                GameManager.PlayerChangeHealth(player, 3 * player.doubleGateAbil);
                break;

            case GateColor.RED:
                player.doubleDamageToCenter = 2; 
                // TODO: damage center for double the roll
                break;

            case GateColor.BLUE:
                player.hasStockade = true;
                break;
        }

        return null;
    }

    IState DoNegativeEffect(PlayerSO player, GateSO gate, int roll)
    {
        switch (gate.Color) 
        {
            case GateColor.BLACK:
                GameManager.PlayerChangeHealth(player, -4 * player.doubleDamageToSelf * player.doubleGateAbil);
                break;

            case GateColor.GREEN:
                /* TODO: center gate gains 3 hp */
                break;

            case GateColor.RED:
                player.doubleDamageToSelf = 2;
                GameManager.PlayerChangeHealth(player, -roll * player.doubleDamageToSelf);
                break;

            case GateColor.BLUE:
                /* TODO: center gate gets sheild */ 
                break;
        }

        return null;
    }

}
