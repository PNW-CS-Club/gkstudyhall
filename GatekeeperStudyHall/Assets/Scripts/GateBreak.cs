using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBreak : MonoBehaviour
{
    
    [SerializeField] PlayerListSO playerListSO;
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
                stateMachine.choosingPlayerState.playerSelect.OnSelect = (selectedPlayer) => {
                    gameManager.PlayerAttacksPlayer(player, selectedPlayer, 4 * player.doubleGateAbil);
                    gameManager.NextTurn();
                };
                return stateMachine.choosingPlayerState;

            case GateColor.GREEN:
                gameManager.PlayerChangeHealth(player, 3 * player.doubleGateAbil);
                break;

            case GateColor.RED:
                player.doubleDamageToCenter = 2; 
                gameManager.PlayerAttacksCenterGate(player, roll * player.doubleDamageToCenter * player.doubleGateAbil);
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
                gameManager.PlayerChangeHealth(player, -4 * player.doubleDamageToSelf * player.doubleGateAbil);
                break;

            case GateColor.GREEN:
                gameManager.HealCenterGate(3 * player.doubleGateAbil);
                break;

            case GateColor.RED:
                player.doubleDamageToSelf = 2;
                gameManager.PlayerChangeHealth(player, -roll * player.doubleDamageToSelf * player.doubleGateAbil);
                break;

            case GateColor.BLUE:
                // random opponent (without a shield) gets shield
                List<PlayerSO> candidates = new();
                foreach (PlayerSO p in playerListSO.list)
                {
                    if (p.isAlive && p != player && !p.hasStockade)
                        candidates.Add(p);
                }

                if (candidates.Count == 0) break;
                
                int randIndex = Random.Range(0, candidates.Count);
                candidates[randIndex].hasStockade = true;
                break;
        }

        return null;
    }

}
